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
    public interface IPartyCategoryRepository :
        IRepository<PartyCategoryDto>,
        IListRepository<PartyCategoryListDto>
    { }

    public class PartyCategoryRepository : IPartyCategoryRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public PartyCategoryRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<PartyCategoryListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<PartyCategoryListDto> categories = _context.PartyCategories
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<PartyCategoryListDto>(_mapper.ConfigurationProvider);

            return categories;
        }

        public async Task<PartyCategoryDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            PartyCategoryDto category = await _context.PartyCategories
                .AsNoTracking()
                .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (category != null)
            {
                return category;
            }
            return null;
            //throw new NotFoundException("Party category not found");
        }

        public async Task<PartyCategoryDto> CreateAsync(Guid orgId, PartyCategoryDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            PartyCategory category = _mapper.Map<PartyCategory>(model);
            category.OrgId = orgId;
            category.DateCreated = DateTime.UtcNow;
            category.DateModified = null;
            await _context.PartyCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartyCategoryDto>(category);
        }

        public async Task UpdateAsync(Guid id, PartyCategoryDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Party category id mismatch");
            }

            PartyCategory category = _mapper.Map<PartyCategory>(model);
            category.DateModified = DateTime.UtcNow;
            _context.PartyCategories.Update(category);
            await _context.SaveChangesAsync();
            //return _mapper.Map<PartyCategoryDto>(category);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(id);
            if (category != null)
            {
                _context.PartyCategories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            PartyCategory category = await _context.PartyCategories
                .FindAsync(status.Id);
            if (category != null)
            {
                category.Active = status.Active;
                _context.PartyCategories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Party category not found");
        }

        public async Task<bool> ExistsAsync(Duplicate data) => await _context.ExistsAsync<PartyCategory>(data);

        public async Task<ValidationResultModel> ValidateAsync(Guid orgId, PartyCategoryDto model)
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