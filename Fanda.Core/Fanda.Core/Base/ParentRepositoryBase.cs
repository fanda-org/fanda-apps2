using AutoMapper;
using Fanda.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public abstract class ParentRepositoryBase<TEntity, TModel, TListModel> :
        RootRepositoryBase<TEntity, TModel, TListModel>, IParentRepositoryBase<TModel, TListModel>
        where TEntity : BaseEntity
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public ParentRepositoryBase(DbContext context, IMapper mapper)
            : base(context, mapper, string.Empty)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            var validationResult = await ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                //return (null, validationResult);
                throw new BadRequestException(validationResult);
            }
            var entity = _mapper.Map<TEntity>(model);
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

        //public virtual async Task UpdateAsync(Guid id, TModel model)
        //{
        //    if (id != model.Id)
        //    {
        //        throw new ArgumentException("Id mismatch");
        //    }
        //    var validationResult = await ValidateAsync(model);
        //    if (!validationResult.IsValid)
        //    {
        //        //return (null, validationResult);
        //        throw new BadRequestException(validationResult);
        //    }

        //    var entity = _mapper.Map<TEntity>(model);
        //    entity.DateModified = DateTime.UtcNow;
        //    _context.Set<TEntity>().Update(entity);
        //    await _context.SaveChangesAsync();
        //}

        public virtual async Task<bool> ExistsAsync(KeyData data)
            => await _context.ExistsAsync<TEntity>(data);

        //public virtual async Task<TModel> GetByAsync(KeyData data)
        //{
        //    var app = await _context.GetByAsync<TEntity>(data);
        //    return _mapper.Map<TModel>(app);
        //}
    }
}
