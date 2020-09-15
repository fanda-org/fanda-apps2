using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Domain;
using FandaAuth.Service.Dto;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IUserRepository :
        //IRepositoryBase<UserDto, UserListDto, UserKeyData>
        ISubRepository<User, UserDto, UserListDto>
    {
    }

    public class UserRepository : ListRepository<User, UserListDto>, IUserRepository
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AuthContext context, IMapper mapper)
            : base(context, mapper, "TenantId == @0")
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new BadRequestException("Id is required");
            }
            var user = await _context.Users
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task<IEnumerable<UserDto>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            var models = await _context.Users
                .AsNoTracking()
                .Where(predicate)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return models;
        }

        public async Task<UserDto> CreateAsync(Guid tenantId, UserDto dto)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new BadRequestException("Tenant id is required");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Password is required");
            }
            var validationResult = await ValidateAsync(tenantId, dto);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
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

        public async Task UpdateAsync(Guid id, UserDto dto)
        {
            if (id != dto.Id)
            {
                throw new BadRequestException("User id mismatch");
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new BadRequestException("Password", "Password is required");
            }

            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
            {
                throw new NotFoundException("User not found");
            }
            var validationResult = await ValidateAsync(dbUser.TenantId, dto);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            PasswordStorage.CreatePasswordHash(dto.Password, out string passwordHash, out string passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.TenantId = dbUser.TenantId;
            user.DateModified = DateTime.UtcNow;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Entry(dbUser).CurrentValues.SetValues(user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new BadRequestException("Id is required");
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

        public virtual async Task<bool> ActivateAsync(Guid id, bool active)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }
            var entity = await _context.Users.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("User not found");
            }
            entity.Active = active;
            entity.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.AnyAsync(predicate);
        }

        public async Task<ValidationErrors> ValidateAsync(Guid tenantId, UserDto model)
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
                model.Errors.AddError(nameof(tenantId), "Tenant id is required");
            }
            bool foundTenant = await _context.Tenants
                .AnyAsync(t => t.Id == tenantId);
            if (!foundTenant)
            {
                model.Errors.AddError(nameof(tenantId), "Tenant not found");
            }

            #endregion Tenant id validation

            #region Validation: Duplicate

            if (await AnyAsync(GetEmailPredicate(model.Email, model.Id)))
            {
                model.Errors.AddError(nameof(model.Email), $"{nameof(model.Email)} '{model.Email}' already exists");
            }
            if (await AnyAsync(GetUserNamePredicate(model.UserName, model.Id)))
            {
                model.Errors.AddError(nameof(model.UserName), $"{nameof(model.UserName)} '{model.UserName}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        #region Private methods

        private ExpressionStarter<User> GetEmailPredicate(string email, Guid id = default)
        {
            var emailExpression = PredicateBuilder.New<User>(e => e.Email == email);
            if (id != null)
            {
                emailExpression = emailExpression.And(e => e.Id != id);
            }
            return emailExpression;
        }

        private ExpressionStarter<User> GetUserNamePredicate(string userName, Guid id = default)
        {
            var userNameExpression = PredicateBuilder.New<User>(e => e.UserName == userName);
            if (id != null)
            {
                userNameExpression = userNameExpression.And(e => e.Id != id);
            }
            return userNameExpression;
        }

        #endregion Private methods
    }
}
