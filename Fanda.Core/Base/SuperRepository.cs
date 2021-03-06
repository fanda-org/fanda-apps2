﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface ISuperRepository<TEntity, TModel, TListModel> : IListRepository<TEntity, TListModel>
    {
        Task<TModel> GetByIdAsync(Guid id);

        Task<IEnumerable<TModel>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        //Task<IEnumerable<TModel>> FindAsync(string expression, params object[] args);

        Task<TModel> CreateAsync(TModel model);

        Task UpdateAsync(Guid id, TModel model);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        //bool Any(string expression, params object[] args);

        Task<bool> ActivateAsync(Guid id, bool active);

        Task<ValidationErrors> ValidateAsync(TModel model);
    }

    public abstract class SuperRepository<TEntity, TModel, TListModel> :
        ListRepository<TEntity, TListModel>, ISuperRepository<TEntity, TModel, TListModel>
        where TEntity : BaseEntity
        where TModel : BaseDto
        where TListModel : class
    {
        private readonly DbContext _context;
        private readonly string _entityTypeName;
        private readonly IMapper _mapper;

        public SuperRepository(DbContext context, IMapper mapper)
            : base(context, mapper /*, null*/ /*string.Empty*/)
        {
            _context = context;
            _mapper = mapper;
            _entityTypeName = typeof(TEntity).Name;
        }

        private DbSet<TEntity> Entities => _context.Set<TEntity>();

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
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

        //public virtual async Task<IEnumerable<TModel>> FindAsync(string expression, params object[] args)
        //{
        //    var models = await Entities
        //        .AsNoTracking()
        //        .Where(expression, args)
        //        .ProjectTo<TModel>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    return models;
        //}

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.DateCreated = DateTime.UtcNow;
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Id is mismatch", nameof(id));
            }

            var dbEntity = await Entities.FindAsync(id);
            if (dbEntity == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }

            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.DateModified = DateTime.UtcNow;
            _context.Entry(dbEntity).CurrentValues.SetValues(model);
            // _context.Update(dbEntity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
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
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
            }

            var entity = await Entities.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"{_entityTypeName} not found");
            }

            entity.Active = active;
            entity.DateModified = DateTime.UtcNow;
            //_context.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        //public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        //    => await Entities.AnyAsync(predicate);

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.AnyAsync(predicate);
        }

        //public virtual bool Any(string expression, params object[] args)
        //{
        //    return Entities.Any(expression, args);
        //}

        public virtual async Task<ValidationErrors> ValidateAsync(TModel model)
        {
            model.Errors.Clear();

            #region Formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting

            #region Check duplicate

            if (await AnyAsync(GetCodePredicate(model.Code, model.Id)))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }

            if (await AnyAsync(GetNamePredicate(model.Name, model.Id)))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Check duplicate

            return model.Errors;
        }

        public override Expression<Func<TEntity, bool>> GetSuperIdPredicate(Guid? superId)
            => x => true;

        private static ExpressionStarter<TEntity> GetCodePredicate(string code, Guid id = default)
        {
            var codeExpression = PredicateBuilder.New<TEntity>(e => e.Code == code);
            if (id != Guid.Empty)
            {
                codeExpression = codeExpression.And(e => e.Id != id);
            }

            return codeExpression;
        }

        private static ExpressionStarter<TEntity> GetNamePredicate(string name, Guid id = default)
        {
            var nameExpression = PredicateBuilder.New<TEntity>(e => e.Name == name);
            if (id != Guid.Empty)
            {
                nameExpression = nameExpression.And(e => e.Id != id);
            }

            return nameExpression;
        }
    }
}