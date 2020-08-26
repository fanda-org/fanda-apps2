using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Domain;
using FandaAuth.Service.Base;
using FandaAuth.Service.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface ITenantRepository :
        IParentRepository<TenantDto>,
        IListRepository<TenantListDto>
    {
    }

    public class TenantRepository :
        RepositoryBase<Tenant, TenantDto, TenantListDto>, ITenantRepository
    {
        public TenantRepository(AuthContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        //public IQueryable<TenantListDto> GetAll(Guid parentId)  // nullable
        //{
        //    IQueryable<TenantListDto> qry = context.Tenants
        //        .AsNoTracking()
        //        .ProjectTo<TenantListDto>(mapper.ConfigurationProvider);
        //    return qry;
        //}

        //public async Task<TenantDto> GetByIdAsync(Guid id/*, bool includeChildren = false*/)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Id is missing");
        //    }
        //    var tenant = await context.Tenants
        //        .AsNoTracking()
        //        .ProjectTo<TenantDto>(mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //    if (tenant == null)
        //    {
        //        throw new NotFoundException("Tenant not found");
        //    }
        //    return tenant;
        //}

        //public async Task<TenantDto> CreateAsync(TenantDto model)
        //{
        //    var tenant = mapper.Map<Tenant>(model);
        //    tenant.DateCreated = DateTime.UtcNow;
        //    tenant.DateModified = null;
        //    await context.Tenants.AddAsync(tenant);
        //    await context.SaveChangesAsync();
        //    return mapper.Map<TenantDto>(tenant);
        //}

        //public async Task UpdateAsync(Guid id, TenantDto model)
        //{
        //    if (id != model.Id)
        //    {
        //        throw new ArgumentException("Tenant id mismatch");
        //    }
        //    var tenant = mapper.Map<Tenant>(model);
        //    tenant.DateModified = DateTime.UtcNow;
        //    context.Tenants.Update(tenant);
        //    await context.SaveChangesAsync();
        //}

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is missing");
        //    }
        //    var tenant = await context.Tenants
        //        .FindAsync(id);
        //    if (tenant == null)
        //    {
        //        throw new NotFoundException("Tenant not found");
        //    }

        //    context.Tenants.Remove(tenant);
        //    await context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is missing");
        //    }

        //    var tenant = await context.Tenants
        //        .FindAsync(status.Id);
        //    if (tenant != null)
        //    {
        //        tenant.Active = status.Active;
        //        tenant.DateModified = DateTime.UtcNow;
        //        context.Tenants.Update(tenant);
        //        await context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new NotFoundException("Tenant not found");
        //}

        //public async Task<bool> ExistsAsync(KeyData data)
        //    => await context.ExistsAsync<Tenant>(data);

        //public async Task<TenantDto> GetByAsync(KeyData data)
        //{
        //    var tenant = await context.ExistsAsync<Tenant>(data);
        //    return mapper.Map<TenantDto>(tenant);
        //}

        //public async Task<ValidationResultModel> ValidateAsync(TenantDto model)
        //{
        //    // Reset validation errors
        //    model.Errors.Clear();

        //    #region Formatting: Cleansing and formatting

        //    model.Code = model.Code.TrimExtraSpaces().ToUpper();
        //    model.Name = model.Name.TrimExtraSpaces();

        //    #endregion Formatting: Cleansing and formatting

        //    #region Validation: Duplicate

        //    // Check email duplicate
        //    var duplCode = new KeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id };
        //    if (await ExistsAsync(duplCode))
        //    {
        //        model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
        //    }
        //    // Check name duplicate
        //    var duplName = new KeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id };
        //    if (await ExistsAsync(duplName))
        //    {
        //        model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
        //    }

        //    #endregion Validation: Duplicate

        //    return model.Errors;
        //}
    }
}
