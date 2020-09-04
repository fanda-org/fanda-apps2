using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Fanda.Core.Base
{
    public abstract class ListRepositoryBase<TEntity, TListModel> : IListRepositoryBase<TListModel>
        where TEntity : BaseEntity
        where TListModel : BaseListDto
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly string _filterByParentId;

        public ListRepositoryBase(DbContext context, IMapper mapper, string filterByParentId)
        {
            _context = context;
            _mapper = mapper;
            _filterByParentId = filterByParentId;
        }

        public virtual IQueryable<TListModel> GetAll(Guid parentId)  // nullable
        {
            if (string.IsNullOrEmpty(_filterByParentId))
            {
                IQueryable<TListModel> qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
            else
            {
                if (parentId == null || parentId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(parentId), $"{nameof(parentId)} is required");
                }
                IQueryable<TListModel> qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .Where(_filterByParentId, parentId)
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
        }
    }
}
