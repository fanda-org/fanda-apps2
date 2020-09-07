﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public abstract class RepositoryBase<TEntity, TModel, TListModel, TKeyData> :
        ListRepositoryBase<TEntity, TListModel>, IRepositoryBase<TModel, TListModel, TKeyData>
        where TEntity : BaseEntity
        where TModel : BaseDto
        where TListModel : BaseListDto
        where TKeyData : KeyData, new()
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public RepositoryBase(DbContext context, IMapper mapper, string filterByParentId)
            : base(context, mapper, filterByParentId)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new BadRequestException("Id is required");
            }

            var model = await _context.Set<TEntity>()
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (model == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }
            return model;
        }

        public virtual async Task<TModel> CreateAsync(TModel model, Guid parentId)
        {
            var validationResult = await ValidateAsync(model, parentId);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            if (parentId != null && parentId != Guid.Empty)
            {
                SetParentId(entity, parentId);
            }
            entity.DateCreated = DateTime.UtcNow;
            entity.DateModified = null;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Id mismatch");
            }

            var dbEntity = await _context.Set<TEntity>()
                .FindAsync(id);
            if (dbEntity == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }

            Guid parentId = GetParentId(dbEntity);
            var validationResult = await ValidateAsync(model, parentId);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.DateModified = DateTime.UtcNow;
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            // _context.Set<TEntity>().Update(dbEntity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new BadRequestException("Id is required");
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
                throw new BadRequestException("Id is required");
            }

            var entity = await _context.Set<TEntity>()
                .FindAsync(status.Id);
            if (entity == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }
            entity.Active = status.Active;
            entity.DateModified = DateTime.UtcNow;
            // _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(TKeyData data)
            => await _context.ExistsAsync<TEntity>(data);

        public virtual async Task<TModel> GetByAsync(TKeyData data)
        {
            var app = await _context.GetByAsync<TEntity>(data);
            return _mapper.Map<TModel>(app);
        }

        public async Task<ValidationErrors> ValidateAsync(TModel model, Guid parentId)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Validation: Duplicate

            // Check code duplicate
            TKeyData keyData = new TKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id };
            if (parentId != null && parentId != Guid.Empty)
            {
                SetParentId(keyData, parentId);
            }
            if (await ExistsAsync(keyData))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            keyData.Field = KeyField.Name; keyData.Value = model.Name;
            if (await ExistsAsync(keyData))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        protected abstract Guid GetParentId(TEntity entity);

        protected abstract void SetParentId(TEntity entity, Guid parentId);

        protected abstract void SetParentId(TKeyData keyData, Guid parentId);
    }
}
