﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public abstract class RootRepositoryBase<TEntity, TModel, TListModel> :
        ListRepositoryBase<TEntity, TListModel>, IRootRepositoryBase<TModel, TListModel>
        where TEntity : BaseEntity
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public RootRepositoryBase(DbContext context, IMapper mapper, string filterByParentId)
            : base(context, mapper, filterByParentId)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }

            var app = await _context.Set<TEntity>()
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (app == null)
            {
                throw new NotFoundException("Application not found");
            }
            return app;
        }

        public virtual async Task UpdateAsync(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Id mismatch");
            }
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                //return (null, validationResult);
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.DateModified = DateTime.UtcNow;
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }
            var entity = await _context.Set<TEntity>()
                .FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }

            var entity = await _context.Set<TEntity>()
                .FindAsync(status.Id);
            if (entity == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }
            entity.Active = status.Active;
            entity.DateModified = DateTime.UtcNow;
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        protected async Task<ValidationErrors> ValidateAsync(TModel model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Validation: Duplicate

            // Check email duplicate
            var duplCode = new KeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new KeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }
    }
}
