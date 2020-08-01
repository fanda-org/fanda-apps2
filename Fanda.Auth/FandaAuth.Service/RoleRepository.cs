using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Domain;
using FandaAuth.Service.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IRoleRepository :
        IRepository<RoleDto>,
        IListRepository<RoleListDto>
    {
        //Task<bool> AddPrivilege(PrivilegeDto model);
        //Task<bool> RemovePrivilege(PrivilegeDto model);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public RoleRepository(AuthContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var role = await context.Roles
                .FindAsync(status.Id);
            if (role != null)
            {
                role.Active = status.Active;
                context.Roles.Update(role);
                await context.SaveChangesAsync();
                return true;
            }
            throw new NotFoundException("Role not found");
        }

        public async Task<RoleDto> CreateAsync(Guid tenantId, RoleDto model)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tenantId), "Tenant id is missing");
            }

            var role = mapper.Map<Role>(model);
            role.TenantId = tenantId;
            role.DateCreated = DateTime.UtcNow;
            role.DateModified = null;
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            return mapper.Map<RoleDto>(role);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }
            var role = await context.Roles
                .FindAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Role not found");
            }

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Duplicate data)
            => await context.ExistsAsync<Role>(data);

        public IQueryable<RoleListDto> GetAll(Guid tenantId)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tenantId), "Tenant id is missing");
            }
            IQueryable<RoleListDto> qry = context.Roles
                .AsNoTracking()
                .Where(v => v.Id == tenantId)
                .ProjectTo<RoleListDto>(mapper.ConfigurationProvider);
            return qry;
        }

        public async Task<RoleDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var role = await context.Roles
                .AsNoTracking()
                .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (role != null)
            {
                return role;
            }
            throw new NotFoundException("Role not found");
        }

        public async Task UpdateAsync(Guid id, RoleDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Role id mismatch");
            }
            var role = mapper.Map<Role>(model);
            role.DateModified = DateTime.UtcNow;
            context.Roles.Update(role);
            await context.SaveChangesAsync();
        }

        public async Task<ValidationResultModel> ValidateAsync(Guid tenantId, RoleDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Validation: Duplicate

            // Check email duplicate
            var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, ParentId = tenantId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, ParentId = tenantId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        // public async Task<bool> AddPrivilege(PrivilegeDto model)
        // {
        //     var appResource = await context.Set<AppResource>()
        //         .FirstOrDefaultAsync(ar => ar.ApplicationId == model.ApplicationId &&
        //             ar.ResourceId == model.ResourceId);
        //     var resourceAction = await context.Set<ResourceAction>()
        //         .FirstOrDefaultAsync(ra => ra.ResourceId == model.ResourceId &&
        //             ra.ActionId == ra.ActionId);

        //     var privilege = new Privilege
        //     {
        //         RoleId = model.RoleId,
        //         AppResourceId = appResource.Id,
        //         ResourceActionId = resourceAction.Id
        //     };
        //     await context.Set<Privilege>().AddAsync(privilege);
        //     await context.SaveChangesAsync();
        //     return true;
        // }

        // public async Task<bool> RemovePrivilege(PrivilegeDto model)
        // {
        //     var privilege = context.Set<Privilege>()
        //         .FirstOrDefault(r => r.RoleId == model.RoleId &&
        //             r.AppResource.ApplicationId == model.ApplicationId &&
        //             r.AppResource.ResourceId == model.ResourceId &&
        //             r.ResourceAction.ResourceId == model.ResourceId &&
        //             r.ResourceAction.ActionId == model.ActionId);
        //     context.Set<Privilege>().Remove(privilege);
        //     await context.SaveChangesAsync();
        //     return true;
        // }
    }
}
