using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface ISubRepository<TEntity, TModel, TListModel> : IListRepository<TListModel>
    {
        Task<TModel> GetByIdAsync(Guid id);

        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TModel> CreateAsync(Guid superId, TModel model);

        Task UpdateAsync(Guid id, TModel model);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ActivateAsync(Guid id, bool active);

        Task<ValidationErrors> ValidateAsync(Guid superId, TModel model);
    }

    public abstract class SubRepository<TEntity, TModel, TListModel> :
        ListRepository<TEntity, TListModel>, ISubRepository<TEntity, TModel, TListModel>
        where TEntity : BaseEntity
        where TModel : BaseDto
        where TListModel : class
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly string _entityTypeName;

        public SubRepository(DbContext context, IMapper mapper, string filterBySuperId)
            : base(context, mapper, filterBySuperId)
        {
            _context = context;
            _mapper = mapper;
            _entityTypeName = typeof(TEntity).Name;
        }

        private DbSet<TEntity> Entities => _context.Set<TEntity>();

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }

            var model = await Entities
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (model == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }

            return model;
        }

        public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var models = await Entities
                .AsNoTracking()
                .Where(predicate)
                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return models;
        }

        public virtual async Task<TModel> CreateAsync(Guid superId, TModel model)
        {
            var validationResult = await ValidateAsync(superId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }
            var entity = _mapper.Map<TEntity>(model);
            SetSuperId(superId, entity);
            entity.DateCreated = DateTime.UtcNow;
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Id is mismatch", "id");
            }
            var dbEntity = await Entities.FindAsync(id);
            if (dbEntity == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }
            Guid superId = GetSuperId(dbEntity);
            var validationResult = await ValidateAsync(superId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }
            var entity = _mapper.Map<TEntity>(model);
            entity.DateModified = DateTime.UtcNow;
            _context.Entry(dbEntity).CurrentValues.SetValues(model);
            _context.Update(dbEntity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }
            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ActivateAsync(Guid id, bool active)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }
            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }
            entity.Active = active;
            entity.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        //public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        //    => await Entities.AnyAsync(predicate);

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.AnyAsync(predicate);
        }

        public virtual async Task<ValidationErrors> ValidateAsync(Guid superId, TModel model)
        {
            model.Errors.Clear();

            #region Formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting

            #region Check duplicate

            if (await AnyAsync(GetCodePredicate(model.Code, superId, model.Id)))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            if (await AnyAsync(GetNamePredicate(model.Name, superId, model.Id)))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Check duplicate

            return model.Errors;
        }

        private ExpressionStarter<TEntity> GetCodePredicate(string code, Guid superId, Guid id = default)
        {
            var codeExpression = PredicateBuilder.New<TEntity>(e => e.Code == code);
            codeExpression = codeExpression.And(GetSuperIdPredicate(superId));
            if (id != null)
            {
                codeExpression = codeExpression.And(e => e.Id != id);
            }
            return codeExpression;
        }

        private ExpressionStarter<TEntity> GetNamePredicate(string name, Guid superId, Guid id = default)
        {
            var nameExpression = PredicateBuilder.New<TEntity>(e => e.Name == name);
            nameExpression = nameExpression.And(GetSuperIdPredicate(superId));
            if (id != null)
            {
                nameExpression = nameExpression.And(e => e.Id != id);
            }
            return nameExpression;
        }

        protected abstract void SetSuperId(Guid superId, TEntity entity);
        protected abstract Guid GetSuperId(TEntity entity);
        protected abstract Expression<Func<TEntity, bool>> GetSuperIdPredicate(Guid superId);
    }
}
