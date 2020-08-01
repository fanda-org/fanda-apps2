using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Models;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Infrastructure.Base;
using Fanda.Infrastructure.Extensions;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Infrastructure
{
    public interface IProductCategoryRepository :
        IRepository<ProductCategoryDto>,
        IListRepository<ProductCategoryListDto>
    { }

    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductCategoryRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<ProductCategoryListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<ProductCategoryListDto> items = _context.ProductCategories
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<ProductCategoryListDto>(_mapper.ConfigurationProvider);

            return items;
        }

        public async Task<ProductCategoryDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var item = await _context.ProductCategories
                .AsNoTracking()
                .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (item != null)
            {
                return item;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<ProductCategoryDto> CreateAsync(Guid orgId, ProductCategoryDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            var item = _mapper.Map<ProductCategory>(model);
            item.OrgId = orgId;
            item.DateCreated = DateTime.UtcNow;
            item.DateModified = null;
            await _context.ProductCategories.AddAsync(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductCategoryDto>(item);
        }

        public async Task UpdateAsync(Guid id, ProductCategoryDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Product category id mismatch");
            }

            var item = _mapper.Map<ProductCategory>(model);
            item.DateModified = DateTime.UtcNow;
            _context.ProductCategories.Update(item);
            await _context.SaveChangesAsync();
            //return _mapper.Map<ProductCategoryDto>(item);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductCategories
                .FindAsync(id);
            if (item != null)
            {
                _context.ProductCategories.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var item = await _context.ProductCategories
                .FindAsync(status.Id);
            if (item != null)
            {
                item.Active = status.Active;
                _context.ProductCategories.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product category not found");
        }

        public async Task<bool> ExistsAsync(Duplicate data) => await _context.ExistsAsync<ProductCategory>(data);

        public async Task<ValidationResultModel> ValidateAsync(Guid orgId, ProductCategoryDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting
            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();
            #endregion

            #region Validation: Duplicate
            // Check code duplicate
            var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }
    }
}