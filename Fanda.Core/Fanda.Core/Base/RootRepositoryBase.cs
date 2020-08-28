using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public abstract class RootRepositoryBase<TEntity, TModel, TListModel> :
        ListRepositoryBase<TEntity, TListModel>, IRootRepository<TModel, TListModel>
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
    }
}
