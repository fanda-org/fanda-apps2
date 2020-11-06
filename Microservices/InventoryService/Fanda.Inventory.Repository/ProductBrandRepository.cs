using System;
using System.Linq.Expressions;
using AutoMapper;
using Fanda.Core.Base;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Domain.Context;
using Fanda.Inventory.Repository.Dto;

namespace Fanda.Inventory.Repository
{
    public interface IProductBrandRepository :
        ISubRepository<ProductBrand, ProductBrandDto, ProductBrandListDto>
    {
    }

    public class ProductBrandRepository :
        SubRepository<ProductBrand, ProductBrandDto, ProductBrandListDto>, IProductBrandRepository
    {
        //private readonly FandaContext _context;
        //private readonly IMapper _mapper;

        public ProductBrandRepository(InvtContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
            //_context = context;
            //_mapper = mapper;
        }

        protected override void SetSuperId(Guid superId, ProductBrand entity)
        {
            entity.OrgId = superId;
        }

        protected override Guid GetSuperId(ProductBrand entity)
        {
            return entity.OrgId;
        }

        protected override Expression<Func<ProductBrand, bool>> GetSuperIdPredicate(Guid superId)
        {
            return pb => pb.OrgId == superId;
        }

        //public IQueryable<ProductBrandListDto> GetAll(Guid orgId)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }
        //    IQueryable<ProductBrandListDto> items = _context.ProductBrands
        //        .AsNoTracking()
        //        .Where(p => p.OrgId == orgId)
        //        .ProjectTo<ProductBrandListDto>(_mapper.ConfigurationProvider);

        //    return items;
        //}

        //public async Task<ProductBrandDto> GetByIdAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Id is required");
        //    }
        //    var item = await _context.ProductBrands
        //        .AsNoTracking()
        //        .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(pc => pc.Id == id);
        //    if (item != null)
        //    {
        //        return item;
        //    }
        //    throw new KeyNotFoundException("Product brand not found");
        //}

        //public async Task<ProductBrandDto> CreateAsync(Guid orgId, ProductBrandDto model)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }

        //    var item = _mapper.Map<ProductBrand>(model);
        //    item.OrgId = orgId;
        //    item.DateCreated = DateTime.UtcNow;
        //    item.DateModified = null;
        //    await _context.ProductBrands.AddAsync(item);
        //    await _context.SaveChangesAsync();
        //    return _mapper.Map<ProductBrandDto>(item);
        //}

        //public async Task UpdateAsync(Guid id, ProductBrandDto model)
        //{
        //    if (id != model.Id)
        //    {
        //        throw new ArgumentException("Product brand id mismatch");
        //    }

        //    var item = _mapper.Map<ProductBrand>(model);
        //    item.DateModified = DateTime.UtcNow;
        //    _context.ProductBrands.Update(item);
        //    await _context.SaveChangesAsync();
        //    //return _mapper.Map<ProductBrandDto>(item);
        //}

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var item = await _context.ProductBrands
        //        .FindAsync(id);
        //    if (item != null)
        //    {
        //        _context.ProductBrands.Remove(item);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Product brand not found");
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var item = await _context.ProductBrands
        //        .FindAsync(status.Id);
        //    if (item != null)
        //    {
        //        item.Active = status.Active;
        //        _context.ProductBrands.Update(item);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Product brand not found");
        //}

        //public async Task<bool> ExistsAsync(OrgKeyData data) => await _context.ExistsAsync<ProductBrand>(data);

        //public async Task<ProductBrandDto> GetByAsync(OrgKeyData data)
        //{
        //    var pb = await _context.GetByAsync<ProductBrand>(data);
        //    return _mapper.Map<ProductBrandDto>(pb);
        //}

        //public async Task<ValidationResultModel> ValidateAsync(Guid orgId, ProductBrandDto model)
        //{
        //    // Reset validation errors
        //    model.Errors.Clear();

        //    #region Formatting: Cleansing and formatting

        //    model.Code = model.Code.ToUpper();
        //    model.Name = model.Name.TrimExtraSpaces();
        //    model.Description = model.Description.TrimExtraSpaces();

        //    #endregion Formatting: Cleansing and formatting

        //    #region Validation: Duplicate

        //    // Check code duplicate
        //    var duplCode = new OrgKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id, OrgId = orgId };
        //    if (await ExistsAsync(duplCode))
        //    {
        //        model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
        //    }
        //    // Check name duplicate
        //    var duplName = new OrgKeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id, OrgId = orgId };
        //    if (await ExistsAsync(duplName))
        //    {
        //        model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
        //    }

        //    #endregion Validation: Duplicate

        //    return model.Errors;
        //}
    }
}