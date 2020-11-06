using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Fanda.Authentication.Domain;
using Fanda.Authentication.Repository.Dto;
using Fanda.Authentication.Repository.ViewModels;
using Fanda.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fanda.Authentication.Repository
{
    public interface IAuthRepository
    {
        //Task<UserDto> LoginAsync(LoginViewModel model);

        Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl);

        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);

        Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress);

        Task<bool> RevokeTokenAsync(string token, string ipAddress);

        /*Task<IEnumerable<ActiveTokenViewModel>> GetRefreshTokens(Guid userId);*/

        Task<ValidationErrors> ValidateAsync(Guid tenantId, RegisterViewModel model);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly AppSettings _appSettings;
        private readonly AuthContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AuthRepository(AuthContext context, IMapper mapper,
            IUserRepository userRepository,
            IOptions<AppSettings> options, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
            _appSettings = options.Value;
            _emailSender = emailSender;
        }

        //public async Task<UserDto> LoginAsync(LoginViewModel model)
        //{
        //    UserDto userModel;
        //    {
        //        User user;
        //        if (RegEx.IsEmail(model.NameOrEmail))
        //        {
        //            user = await _context.Users
        //                .FirstOrDefaultAsync(x => x.Email == model.NameOrEmail);
        //        }
        //        else
        //        {
        //            user = await _context.Users
        //                .FirstOrDefaultAsync(x => x.UserName == model.NameOrEmail);
        //        }
        //        // return null if user not found
        //        if (user == null)
        //        {
        //            return null;
        //        }

        //        // check if password is correct
        //        if (!PasswordStorage.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
        //        {
        //            return null;
        //        }

        //        userModel = _mapper.Map<UserDto>(user);
        //    }
        //    return userModel;
        //}

        public async Task<UserDto> RegisterAsync(RegisterViewModel model, string callbackUrl)
        {
            PasswordStorage.CreatePasswordHash(model.Password, out string passwordHash, out string passwordSalt);

            UserDto userModel;
            {
                var user = _mapper.Map<User>(model);
                user.DateCreated = DateTime.UtcNow;
                user.DateModified = null;
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                // user.Active = true;
                user.ResetPassword = true;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                userModel = _mapper.Map<UserDto>(user);
            }

            // Ignore if error occurred while sending email
            try
            {
                await _emailSender.SendGridEmailAsync(model.Email, "Fanda: Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            catch { }

            return userModel;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == model.UserName);

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
            string jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(ipAddress);

            // save refresh token
            user.DateLastLogin = DateTime.UtcNow;
            user.RefreshTokens.Add(refreshToken);
            // _context.Update(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, user.TenantId, jwtToken, refreshToken.Token, false);
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token, string ipAddress)
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
            // _context.Update(user);
            await _context.SaveChangesAsync();

            // generate new jwt
            string jwtToken = GenerateJwtToken(user);

            var userDto = _mapper.Map<UserDto>(user);
            return new AuthenticateResponse(userDto, user.TenantId, jwtToken, newRefreshToken.Token, false);
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress)
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
            // _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<IEnumerable<ActiveTokenViewModel>> GetRefreshTokens(Guid userId)
        //{
        //    //var result =
        //    //    (from u in _context.Users
        //    //     where u.Id.Equals(userId) && u.RefreshTokens.Any(t => t.Revoked==null && t.Expires >= DateTime.UtcNow)
        //    //     select u.RefreshTokens)
        //    //    .AsNoTracking()
        //    //    .ToList();

        //    //var user = await _context.Users
        //    //    .Include(u => u.RefreshTokens)
        //    //    //.Where(t => t.RefreshTokens.Any(r => r.Revoked == null && r.Expires >= DateTime.UtcNow))
        //    //    .AsNoTracking()
        //    //    .FirstOrDefaultAsync(u => u.Id == userId);

        //    var tokens = await _context.RefreshTokens
        //        .Where(t => t.UserId == userId && t.DateRevoked == null && t.DateExpires >= DateTime.UtcNow)
        //        .ProjectTo<ActiveTokenViewModel>(_mapper.ConfigurationProvider)
        //        .ToListAsync();
        //    //var result = user.RefreshTokens.Where(t => t.IsActive);
        //    //return _mapper.Map<IEnumerable<RefreshTokenDto>>(result);

        //    //string sql =
        //    //    "SELECT r.Id, r.UserId, r.Token, r.Expires, r.Created, r.CreatedByIp, r.Revoked, r.RevokedByIp, r.ReplacedByToken " +
        //    //    "FROM RefreshTokens r WITH (NOLOCK)" +
        //    //    "WHERE r.UserId = @UserId " +
        //    //    "AND r.Revoked IS NULL AND r.Expires >= GETUTCDATE()";
        //    //var result = await _dbClient.Connection.QueryAsync<RefreshTokenDto>(sql, new { UserId = userId });

        //    return tokens; //_mapper.Map<IEnumerable<RefreshTokenDto>>(user.RefreshTokens.Where(t => t.Revoked == null && t.Expires >= DateTime.UtcNow));
        //}

        public async Task<ValidationErrors> ValidateAsync(Guid tenantId, RegisterViewModel model)
        {
            var userDto = _mapper.Map<UserDto>(model);
            return await _userRepository.ValidateAsync(tenantId, userDto);
        }

        #region Privates

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.FandaSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                    //JwtRegisteredClaimNames.Sub, JwtRegisteredClaimNames.NameId, JwtRegisteredClaimNames.Email
                }),
                Expires = DateTime.UtcNow.AddMinutes(180),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static UserToken GenerateRefreshToken(string ipAddress)
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
    }
}