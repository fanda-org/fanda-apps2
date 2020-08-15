using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Domain;
using FandaAuth.Service.Base;
using FandaAuth.Service.Dto;
using FandaAuth.Service.Extensions;
using FandaAuth.Service.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IUserRepository :
        IUserRepository<UserDto>,
        IListRepository<UserListDto>
    {
        Task<ValidationResultModel> ValidateAsync(Guid tenantId, RegisterViewModel model);

        Task<UserDto> LoginAsync(LoginViewModel model);

        Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl);

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);

        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);

        Task<bool> RevokeToken(string token, string ipAddress);

        Task<IEnumerable<ActiveTokenDto>> GetRefreshTokens(Guid userId);

        bool MapOrgAsync(Guid userId, Guid orgId);

        bool UnmapOrgAsync(Guid userId, Guid orgId);

        bool MapRoleAsync(Guid userId, string roleName, Guid orgId);

        bool UnmapRoleAsync(Guid userId, Guid roleId, Guid orgId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserRepository> _logger;
        private readonly AppSettings _appSettings;
        //private readonly IDbClient _dbClient;

        public UserRepository(AuthContext context, IMapper mapper,
            IEmailSender emailSender, ILogger<UserRepository> logger,
            IOptions<AppSettings> options/*, IDbClient dbClient*/)
        {
            _context = context;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
            _appSettings = options.Value;
            //_dbClient = dbClient;
        }

        public async Task<UserDto> LoginAsync(LoginViewModel model)
        {
            UserDto userModel;
            {
                User user;
                if (RegEx.IsEmail(model.NameOrEmail))
                {
                    user = await _context.Users
                        .FirstOrDefaultAsync(x => x.Email == model.NameOrEmail);
                }
                else
                {
                    user = await _context.Users
                        .FirstOrDefaultAsync(x => x.UserName == model.NameOrEmail);
                }
                // return null if user not found
                if (user == null)
                {
                    return null;
                }

                // check if password is correct
                if (!PasswordStorage.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }

                userModel = _mapper.Map<UserDto>(user);
            }
            return userModel;
        }

        public async Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl)
        {
            PasswordStorage.CreatePasswordHash(model.Password, out string passwordHash, out string passwordSalt);

            UserDto userModel;
            {
                User user = _mapper.Map<User>(model);
                user.DateCreated = DateTime.UtcNow;
                user.DateModified = null;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                // user.Active = true;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                userModel = _mapper.Map<UserDto>(user);
            }

            // Ignore if error occurred while sending email
            try
            {
                await _emailSender.SendEmailAsync(model.Email, "Fanda: Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            catch { }

            return userModel;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == model.Username);

            // return null if user not found
            if (user == null)
            {
                return null;
            }

            // check if password is correct
            if (!PasswordStorage.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            user.DateLastLogin = DateTime.UtcNow;
            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, user.TenantId, jwtToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null)
            {
                return null;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive)
            {
                return null;
            }

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.DateRevoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            // generate new jwt
            var jwtToken = GenerateJwtToken(user);

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, user.TenantId, jwtToken, newRefreshToken.Token);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null)
            {
                return false;
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive)
            {
                return false;
            }

            // revoke token and save
            refreshToken.DateRevoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ActiveTokenDto>> GetRefreshTokens(Guid userId)
        {
            //var result =
            //    (from u in _context.Users
            //     where u.Id.Equals(userId) && u.RefreshTokens.Any(t => t.Revoked==null && t.Expires >= DateTime.UtcNow)
            //     select u.RefreshTokens)
            //    .AsNoTracking()
            //    .ToList();

            //var user = await _context.Users
            //    .Include(u => u.RefreshTokens)
            //    //.Where(t => t.RefreshTokens.Any(r => r.Revoked == null && r.Expires >= DateTime.UtcNow))
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(u => u.Id == userId);

            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && t.DateRevoked == null && t.DateExpires >= DateTime.UtcNow)
                .ProjectTo<ActiveTokenDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            //var result = user.RefreshTokens.Where(t => t.IsActive);
            //return _mapper.Map<IEnumerable<RefreshTokenDto>>(result);

            //string sql =
            //    "SELECT r.Id, r.UserId, r.Token, r.Expires, r.Created, r.CreatedByIp, r.Revoked, r.RevokedByIp, r.ReplacedByToken " +
            //    "FROM RefreshTokens r WITH (NOLOCK)" +
            //    "WHERE r.UserId = @UserId " +
            //    "AND r.Revoked IS NULL AND r.Expires >= GETUTCDATE()";
            //var result = await _dbClient.Connection.QueryAsync<RefreshTokenDto>(sql, new { UserId = userId });

            return tokens; //_mapper.Map<IEnumerable<RefreshTokenDto>>(user.RefreshTokens.Where(t => t.Revoked == null && t.Expires >= DateTime.UtcNow));
        }

        public IQueryable<UserListDto> GetAll(Guid tenantId)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException("tenantId", "Tenant id is missing");
            }
            IQueryable<UserListDto> userQry = _context.Users
                .AsNoTracking()
                //.Include(u => u.OrgUsers)
                //.ThenInclude(ou => ou.Organization)
                //.SelectMany(u => u.OrgUsers.Select(ou => ou.Organization))
                //.Where(u => u.OrgUsers.Any(ou => ou.OrgId == orgId))
                .Where(u => u.TenantId == tenantId)
                .ProjectTo<UserListDto>(_mapper.ConfigurationProvider);
            //.Where(u => u.OrgId == orgId);
            //if (orgId != null && orgId != Guid.Empty)
            //{
            //    userQry = userQry.Where(u => u.OrgId == orgId);
            //}
            return userQry;
        }

        public async Task<UserDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var user = await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            throw new NotFoundException("User not found");
        }

        public async Task<UserDto> CreateAsync(Guid tenantId, UserDto dto)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tenantId), "Tenant id is missing");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is missing");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out string passwordHash, out string passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.TenantId = tenantId;
            user.DateCreated = DateTime.UtcNow;
            user.DateModified = null;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateAsync(Guid userId, UserDto dto)
        {
            if (userId != dto.Id)
            {
                throw new ArgumentException("User id mismatch");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is missing");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out string passwordHash, out string passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.DateModified = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            //return _mapper.Map<UserDto>(user);
        }

        public bool MapOrgAsync(Guid userId, Guid orgId)
        {
            throw new NotImplementedException();

            // if (userId == null || userId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("userId", "User Id is missing");
            // }
            // if (orgId == null || orgId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("orgId", "Org Id is missing");
            // }
            // var OrgUsers = _context.Set<OrgUser>();

            // var orgUser = await OrgUsers
            //     .FindAsync(orgId, userId);
            // if (orgUser == null)
            // {
            //     await OrgUsers.AddAsync(new OrgUser
            //     {
            //         UserId = userId,
            //         OrgId = orgId
            //     });
            //     await _context.SaveChangesAsync();
            // }
            //return true;
        }

        public bool UnmapOrgAsync(Guid userId, Guid orgId)
        {
            throw new NotImplementedException();
            // if (userId == null || userId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("userId", "User Id is missing");
            // }
            // if (orgId == null || orgId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("orgId", "Org Id is missing");
            // }
            // var OrgUsers = _context.Set<OrgUser>();

            // var orgUser = await OrgUsers
            //     .FindAsync(orgId, userId);

            // if (orgUser == null)
            // {
            //     throw new KeyNotFoundException("User not found in organization");
            // }

            // OrgUsers.Remove(orgUser);
            // await _context.SaveChangesAsync();
            //return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }
            var user = await _context.Users
                .FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool MapRoleAsync(Guid userId, string roleName, Guid orgId)
        {
            throw new NotImplementedException();
            // try
            // {
            //     if (userId == null || userId == Guid.Empty)
            //     {
            //         throw new ArgumentNullException("userId", "User Id is missing");
            //     }
            //     if (string.IsNullOrEmpty(roleName))
            //     {
            //         throw new ArgumentNullException("roleName", "Role Name is missing");
            //     }
            //     if (orgId == null || orgId == Guid.Empty)
            //     {
            //         throw new ArgumentNullException("orgId", "Role Id is missing");
            //     }

            // Role role = await _context.Roles
            //     .AsNoTracking()
            //     .FirstOrDefaultAsync(r => r.Name == roleName && r.OrgId == orgId);
            // if (role != null)
            // {
            //     await _context.Set<OrgUserRole>().AddAsync(new OrgUserRole
            //     {
            //         OrgId = orgId,
            //         UserId = userId,
            //         RoleId = role.Id
            //     });
            //     await _context.SaveChangesAsync();
            // }
            //     return true;
            // }
            //     catch (Exception ex)
            //     {
            //         _logger.LogError(ex, ex.Message);
            //     }
            //     return false;
        }

        public bool UnmapRoleAsync(Guid userId, Guid roleId, Guid orgId)
        {
            throw new NotImplementedException();
            // if (userId == null || userId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("userId", "User Id is missing");
            // }
            // if (roleId == null || roleId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("roleId", "Role Id is missing");
            // }
            // if (orgId == null || orgId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("orgId", "Role Id is missing");
            // }
            // var OrgUserRoles = _context.Set<OrgUserRole>();

            // var orgUserRole = await OrgUserRoles
            //     .FindAsync(orgId, userId, roleId);

            // if (orgUserRole == null)
            // {
            //     throw new KeyNotFoundException("User not found in organization");
            // }

            // OrgUserRoles.Remove(orgUserRole);
            // await _context.SaveChangesAsync();
            //return true;
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var user = await _context.Users
                .FindAsync(status.Id);
            if (user != null)
            {
                user.Active = status.Active;
                user.DateModified = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("User not found");
        }

        public async Task<bool> ExistsAsync(UserKeyData data)
            => await _context.ExistsAsync<User>(data);

        public async Task<UserDto> GetByAsync(UserKeyData data)
        {
            var user = await _context.ExistsAsync<User>(data);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<ValidationResultModel> ValidateAsync(Guid tenantId, RegisterViewModel model)
        {
            var userDto = _mapper.Map<UserDto>(model);
            return await ValidateAsync(tenantId, userDto);
        }

        public async Task<ValidationResultModel> ValidateAsync(Guid tenantId, UserDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Email = model.Email.TrimExtraSpaces();
            if (!EmailHelper.IsValid(model.Email))
            {
                model.Errors.AddError(nameof(model.Email), $"{model.Email} is not valid email format");
            }
            model.UserName = model.UserName.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Tenant id validation

            if (tenantId == null || tenantId == Guid.Empty)
            {
                //throw new ArgumentNullException(nameof(tenantId), "Tenant id is missing");
                model.Errors.AddError(nameof(tenantId), "Tenant id is missing");
            }
            //if (tenantId != model.TenantId)
            //{
            //    //throw new ArgumentException("Tenant id is mismatch");
            //    model.Errors.AddError(nameof(tenantId), "Tenant id is mismatch");
            //}
            var dbTenant = await _context.Tenants
                .FindAsync(tenantId);
            if (dbTenant == null)
            {
                model.Errors.AddError(nameof(tenantId), "Tenant not found");
            }

            #endregion Tenant id validation

            #region Validation: Duplicate

            // Check email duplicate
            var duplEmail = new UserKeyData { Field = KeyField.Email, Value = model.Email, Id = model.Id, TenantId = tenantId };
            if (await ExistsAsync(duplEmail))
            {
                model.Errors.AddError(nameof(model.Email), $"{nameof(model.Email)} '{model.Email}' already exists");
            }
            // Check name duplicate
            var duplName = new UserKeyData { Field = KeyField.Name, Value = model.UserName, Id = model.Id, TenantId = tenantId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.UserName), $"{nameof(model.UserName)} '{model.UserName}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        #region Privates

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.FandaSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                    //JwtRegisteredClaimNames.Sub, JwtRegisteredClaimNames.NameId, JwtRegisteredClaimNames.Email
                }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private UserToken GenerateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new UserToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    DateExpires = DateTime.UtcNow.AddDays(7),
                    DateCreated = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        #endregion Privates

        #region Role specific

        //public async Task<ViewModel.Access.IdentityResult> AddToRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    //Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRoleAsync(user, role);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public async Task<ViewModel.Access.IdentityResult> AddToRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.AddToRolesAsync(user, roles);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public Task<IList<string>> GetRolesAsync(UserViewModel userVM)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    return _userManager.GetRolesAsync(user);
        //}

        //public async Task<IList<UserViewModel>> GetUsersInRoleAsync(string roleName)
        //{
        //    var users = await _userManager.GetUsersInRoleAsync(roleName);
        //    return _mapper.Map<IList<UserViewModel>>(users);
        //}

        //public Task<bool> IsInRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    return _userManager.IsInRoleAsync(user, role);
        //}

        //public async Task<ViewModel.Access.IdentityResult> RemoveFromRoleAsync(UserViewModel userVM, string role)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        //public async Task<ViewModel.Access.IdentityResult> RemoveFromRolesAsync(UserViewModel userVM, IEnumerable<string> roles)
        //{
        //    var user = _mapper.Map<User>(userVM);
        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.RemoveFromRolesAsync(user, roles);
        //    return new ViewModel.Access.IdentityResult
        //    {
        //        Succeeded = result.Succeeded,
        //        Errors = result.Errors.Select(e =>
        //            new ViewModel.Access.IdentityError
        //            {
        //                Code = e.Code,
        //                Description = e.Description
        //            })
        //    };
        //}

        #endregion Role specific
    }
}
