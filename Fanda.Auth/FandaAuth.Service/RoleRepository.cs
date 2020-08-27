using AutoMapper;
using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Domain;
using FandaAuth.Service.Base;
using FandaAuth.Service.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IRoleRepository :
        ITenantRepository<RoleDto>,
        IListRepository<RoleListDto>
    {
    }

    public class RoleRepository :
        TenantRepositoryBase<Role, RoleDto, RoleListDto>,
        IRoleRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public RoleRepository(AuthContext context, IMapper mapper)
            : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        //public IQueryable<RoleListDto> GetAll(Guid tenantId)
        //{
        //    if (tenantId == null || tenantId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(tenantId), "Tenant id is required");
        //    }
        //    IQueryable<RoleListDto> qry = context.Roles
        //        .AsNoTracking()
        //        .Where(r => r.TenantId == tenantId)
        //        .ProjectTo<RoleListDto>(mapper.ConfigurationProvider);
        //    return qry;
        //}

        //public async Task<RoleDto> GetByIdAsync(Guid id/*, bool includeChildren = false*/)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Id is required");
        //    }

        //    var roleBase = await context.Roles
        //        .AsNoTracking()
        //        .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //    var role = mapper.Map<RoleDto>(roleBase);

        //    if (role == null)
        //    {
        //        throw new NotFoundException("Role not found");
        //    }
        //    //else if (!includeChildren)
        //    //{
        //    return role;
        //    //}

        //    //role.Privileges = await context.Set<RolePrivilege>()
        //    //    .AsNoTracking()
        //    //    .Where(m => m.RoleId == id)
        //    //    //.SelectMany(oc => oc.AppResources.Select(c => c.Resource))
        //    //    .ProjectTo<RolePrivilegeDto>(mapper.ConfigurationProvider)
        //    //    .ToListAsync();
        //    //return role;
        //}

        //public async Task<RoleChildrenDto> GetChildrenByIdAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var rolePrivileges = new RoleChildrenDto
        //    {
        //        Privileges = await context.Set<RolePrivilege>()
        //            .AsNoTracking()
        //            .Where(m => m.RoleId == id)
        //            //.SelectMany(oc => oc.AppResources.Select(c => c.Resource))
        //            .ProjectTo<RolePrivilegeDto>(mapper.ConfigurationProvider)
        //            .ToListAsync()
        //    };
        //    return rolePrivileges;
        //}

        //public async Task<RoleDto> CreateAsync(Guid tenantId, RoleDto model)
        //{
        //    if (tenantId == null || tenantId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(tenantId), "Tenant id is required");
        //    }

        //    var role = mapper.Map<Role>(model);
        //    role.TenantId = tenantId;
        //    role.DateCreated = DateTime.UtcNow;
        //    role.DateModified = null;
        //    await context.Roles.AddAsync(role);
        //    await context.SaveChangesAsync();
        //    return mapper.Map<RoleDto>(role);
        //}

        public async override Task UpdateAsync(Guid id, RoleDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Role id mismatch");
            }

            Role role = mapper.Map<Role>(model);
            Role dbRole = await context.Roles
                .Where(o => o.Id == role.Id)
                .Include(o => o.Privileges)   //.ThenInclude(oc => oc.Resource)
                .FirstOrDefaultAsync();

            if (dbRole == null)
            {
                //org.DateCreated = DateTime.UtcNow;
                //org.DateModified = null;
                //await _context.Organizations.AddAsync(org);
                throw new NotFoundException("Role not found");
            }

            try
            {
                // delete all app-resource that are no longer exists
                foreach (RolePrivilege dbRolePrivilege in dbRole.Privileges)
                {
                    //Resource dbResource = dbAppResource.Resource;
                    //if (app.AppResources.All(oc => oc.Resource.Id != dbAppResource.Resource.Id))
                    if (role.Privileges.All(ar => ar.RoleId != dbRolePrivilege.RoleId)) // && ar.AppResourceId != dbRolePrivilege.AppResourceId
                    {
                        //context.Resources.Remove(dbResource);
                        context.Set<RolePrivilege>().Remove(dbRolePrivilege);
                    }
                }
            }
            catch { }

            // copy current (incoming) values to db
            role.DateModified = DateTime.UtcNow;
            context.Entry(dbRole).CurrentValues.SetValues(role);

            #region Resources

            var resourcePairs = from curr in role.Privileges   //.Select(oc => oc.Resource)
                                join db in dbRole.Privileges   //.Select(oc => oc.Resource)
                                     on curr.RoleId equals db.RoleId into grp
                                from db in grp.DefaultIfEmpty()
                                select new { curr, db };
            foreach (var pair in resourcePairs)
            {
                if (pair.db != null)
                {
                    context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    context.Set<RolePrivilege>().Update(pair.db);
                }
                else
                {
                    //var appResource = new RolePrivilege
                    //{
                    //    RoleId = role.Id,
                    //    AppResourceId =
                    //    //ResourceId = pair.curr.Id,
                    //};
                    //dbRole.Privileges.Add(appResource);
                    context.Set<RolePrivilege>().Add(pair.curr);
                }
            }

            #endregion Resources

            context.Roles.Update(dbRole);
            await context.SaveChangesAsync();
        }

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }
        //    var role = await context.Roles
        //        .FindAsync(id);
        //    if (role == null)
        //    {
        //        throw new NotFoundException("Role not found");
        //    }

        //    context.Roles.Remove(role);
        //    await context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var role = await context.Roles
        //        .FindAsync(status.Id);
        //    if (role != null)
        //    {
        //        role.Active = status.Active;
        //        role.DateModified = DateTime.UtcNow;
        //        context.Roles.Update(role);
        //        await context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new NotFoundException("Role not found");
        //}

        //public async Task<bool> ExistsAsync(TenantKeyData data)
        //    => await context.ExistsAsync<Role>(data);

        //public async Task<RoleDto> GetByAsync(TenantKeyData data)
        //{
        //    var role = await context.GetByAsync<Role>(data);
        //    return mapper.Map<RoleDto>(role);
        //}

        //public async Task<ValidationResultModel> ValidateAsync(Guid tenantId, RoleDto model)
        //{
        //    // Reset validation errors
        //    model.Errors.Clear();

        //    #region Formatting: Cleansing and formatting

        //    model.Code = model.Code.TrimExtraSpaces().ToUpper();
        //    model.Name = model.Name.TrimExtraSpaces();

        //    #endregion Formatting: Cleansing and formatting

        //    #region Validation: Duplicate

        //    // Check email duplicate
        //    var duplCode = new TenantKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id, TenantId = tenantId };
        //    if (await ExistsAsync(duplCode))
        //    {
        //        model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
        //    }
        //    // Check name duplicate
        //    var duplName = new TenantKeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id, TenantId = tenantId };
        //    if (await ExistsAsync(duplName))
        //    {
        //        model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
        //    }

        //    #endregion Validation: Duplicate

        //    return model.Errors;
        //}

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
