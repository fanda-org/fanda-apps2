using AutoMapper;
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
        private DbContext _context;
        private IMapper _mapper;

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

        public virtual async Task<TModel> CreateAsync(TModel model, Guid parentId)
        {
            var validationResult = await ValidateAsync(model, parentId);
            if (!validationResult.IsValid)
            {
                //return (null, validationResult);
                throw new BadRequestException(validationResult);
            }
            var entity = _mapper.Map<TEntity>(model);
            if (parentId != null && parentId != Guid.Empty)
            {
                SetParentId(entity, parentId);
            }
            entity.DateCreated = DateTime.UtcNow;
            entity.DateModified = null;
            //foreach (var ar in app.AppResources)
            //{
            //    ar.DateCreated = DateTime.UtcNow;
            //    ar.DateModified = null;
            //}
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(TModel model, Guid parentId)
        {
            //if (id != model.Id)
            //{
            //    throw new ArgumentException("Id mismatch");
            //}
            var dbEntity = _context.Set<TEntity>().Find(model.Id);
            if (dbEntity == null)
            {
                throw new NotFoundException($"{nameof(TEntity)} not found");
            }

            //ValidationErrors validationResult;
            //if (parentId == null || parentId == Guid.Empty)
            //{
            var validationResult = await ValidateAsync(model, parentId);
            //}
            //else
            //{
            //    validationResult = await ValidateWithParentId(model, parentId);
            //}

            if (!validationResult.IsValid)
            {
                //return (null, validationResult);
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            dbEntity.DateModified = DateTime.UtcNow;
            _context.Set<TEntity>().Update(dbEntity);
            await _context.SaveChangesAsync();
        }

        //protected abstract Task<ValidationErrors> ValidateWithParentId(TModel model, Guid parentId);

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

        protected abstract void SetParentId(TEntity entity, Guid parentId);

        protected abstract void SetParentId(TKeyData keyData, Guid parentId);
    }
}
