using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Domain;
using FandaAuth.Service.Base;
using FandaAuth.Service.Dto;
using FandaAuth.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IUserRepository :
        IRepositoryBase<UserDto, UserListDto, UserKeyData>
    {
    }

    public class UserRepository : ListRepositoryBase<User, UserListDto>, IUserRepository
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AuthContext context, IMapper mapper)
            : base(context, mapper, "TenantId == '{0}'")
        {
            _context = context;
            _mapper = mapper;
        }

        //public IQueryable<UserListDto> GetAll(Guid tenantId, Query queryInput)
        //{
        //    if (tenantId == null || tenantId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("tenantId", "Tenant id is required");
        //    }
        //    IQueryable<UserListDto> userQry = _context.Users
        //        .AsNoTracking()
        //        .Where(u => u.TenantId == tenantId)
        //        .ProjectTo<UserListDto>(_mapper.ConfigurationProvider);
        //    return userQry;
        //}

        public async Task<UserDto> GetByIdAsync(Guid id/*, bool includeChildren = false*/)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }
            var user = await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task<UserDto> CreateAsync(UserDto dto, Guid tenantId)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tenantId), "Tenant id is required");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is required");
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

        public async Task UpdateAsync(UserDto dto, Guid tenantId)
        {
            //if (userId != dto.Id)
            //{
            //    throw new ArgumentException("User id mismatch");
            //}
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentNullException("Password", "Password is required");
            }

            var dbUser = await _context.Users.FindAsync(dto.Id);
            if (dbUser == null)
            {
                throw new NotFoundException("User not found");
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out string passwordHash, out string passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.DateModified = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            // _context.Users.Update(user);
            _context.Entry(dbUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            //return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }
            var user = await _context.Users
                .FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //public bool MapOrgAsync(Guid userId, Guid orgId)
        //{
        //    throw new NotImplementedException();

        //    // if (userId == null || userId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("userId", "User Id is required");
        //    // }
        //    // if (orgId == null || orgId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("orgId", "Org Id is required");
        //    // }
        //    // var OrgUsers = _context.Set<OrgUser>();

        //    // var orgUser = await OrgUsers
        //    //     .FindAsync(orgId, userId);
        //    // if (orgUser == null)
        //    // {
        //    //     await OrgUsers.AddAsync(new OrgUser
        //    //     {
        //    //         UserId = userId,
        //    //         OrgId = orgId
        //    //     });
        //    //     await _context.SaveChangesAsync();
        //    // }
        //    //return true;
        //}

        //public bool UnmapOrgAsync(Guid userId, Guid orgId)
        //{
        //    throw new NotImplementedException();
        //    // if (userId == null || userId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("userId", "User Id is required");
        //    // }
        //    // if (orgId == null || orgId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("orgId", "Org Id is required");
        //    // }
        //    // var OrgUsers = _context.Set<OrgUser>();

        //    // var orgUser = await OrgUsers
        //    //     .FindAsync(orgId, userId);

        //    // if (orgUser == null)
        //    // {
        //    //     throw new KeyNotFoundException("User not found in organization");
        //    // }

        //    // OrgUsers.Remove(orgUser);
        //    // await _context.SaveChangesAsync();
        //    //return true;
        //}

        //public bool MapRoleAsync(Guid userId, string roleName, Guid orgId)
        //{
        //    throw new NotImplementedException();
        //    // try
        //    // {
        //    //     if (userId == null || userId == Guid.Empty)
        //    //     {
        //    //         throw new ArgumentNullException("userId", "User Id is required");
        //    //     }
        //    //     if (string.IsNullOrEmpty(roleName))
        //    //     {
        //    //         throw new ArgumentNullException("roleName", "Role Name is required");
        //    //     }
        //    //     if (orgId == null || orgId == Guid.Empty)
        //    //     {
        //    //         throw new ArgumentNullException("orgId", "Role Id is required");
        //    //     }

        //    // Role role = await _context.Roles
        //    //     .AsNoTracking()
        //    //     .FirstOrDefaultAsync(r => r.Name == roleName && r.OrgId == orgId);
        //    // if (role != null)
        //    // {
        //    //     await _context.Set<OrgUserRole>().AddAsync(new OrgUserRole
        //    //     {
        //    //         OrgId = orgId,
        //    //         UserId = userId,
        //    //         RoleId = role.Id
        //    //     });
        //    //     await _context.SaveChangesAsync();
        //    // }
        //    //     return true;
        //    // }
        //    //     catch (Exception ex)
        //    //     {
        //    //         _logger.LogError(ex, ex.Message);
        //    //     }
        //    //     return false;
        //}

        //public bool UnmapRoleAsync(Guid userId, Guid roleId, Guid orgId)
        //{
        //    throw new NotImplementedException();
        //    // if (userId == null || userId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("userId", "User Id is required");
        //    // }
        //    // if (roleId == null || roleId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("roleId", "Role Id is required");
        //    // }
        //    // if (orgId == null || orgId == Guid.Empty)
        //    // {
        //    //     throw new ArgumentNullException("orgId", "Role Id is required");
        //    // }
        //    // var OrgUserRoles = _context.Set<OrgUserRole>();

        //    // var orgUserRole = await OrgUserRoles
        //    //     .FindAsync(orgId, userId, roleId);

        //    // if (orgUserRole == null)
        //    // {
        //    //     throw new KeyNotFoundException("User not found in organization");
        //    // }

        //    // OrgUserRoles.Remove(orgUserRole);
        //    // await _context.SaveChangesAsync();
        //    //return true;
        //}

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
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
            throw new NotFoundException("User not found");
        }

        public async Task<bool> ExistsAsync(UserKeyData data)
            => await _context.ExistsAsync<User>(data);

        public async Task<UserDto> GetByAsync(UserKeyData data)
        {
            var user = await _context.ExistsAsync<User>(data);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<ValidationErrors> ValidateAsync(UserDto model, Guid tenantId)
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
                //throw new ArgumentNullException(nameof(tenantId), "Tenant id is required");
                model.Errors.AddError(nameof(tenantId), "Tenant id is required");
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
