using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public abstract class ListRepositoryBase<TEntity, TListModel> : IListRepositoryBase<TListModel>
        where TEntity : class
        where TListModel : class
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

        public virtual async Task<DataResponse<IEnumerable<TListModel>>> GetAll(Guid parentId, Query queryInput)  // nullable
        {
            if (queryInput.Page > 0 || queryInput.PageSize > 0)
            {
                return await GetPaged(parentId, queryInput);
            }
            else
            {
                return await GetList(parentId, queryInput);
            }
        }

        private async Task<PagedResponse<IEnumerable<TListModel>>> GetPaged(Guid parentId, Query queryInput)
        {
            var (list, itemsCount) = await Execute(parentId, queryInput);
            var response = PagedResponse<IEnumerable<TListModel>>.Succeeded(list, itemsCount, queryInput.Page, queryInput.PageSize);
            return response;
        }

        private async Task<DataResponse<IEnumerable<TListModel>>> GetList(Guid parentId, Query queryInput)
        {
            var (list, _) = await Execute(parentId, queryInput, true);
            var response = DataResponse<IEnumerable<TListModel>>.Succeeded(list);
            return response;
        }

        private async Task<(IEnumerable<TListModel>, int)> Execute(Guid parentId, Query queryInput, bool ignoreCount = false)
        {
            var (qry, itemsCount) = GenerateQuery(parentId, queryInput, ignoreCount);
            var list = await qry.ToListAsync();
            return (list, itemsCount);
        }

        private (IQueryable<TListModel>, int) GenerateQuery(Guid parentId, Query queryInput, bool ignoreCount = false)
        {
            var dbQuery = GetBaseQuery(parentId);
            if (!string.IsNullOrEmpty(queryInput.Filter))
            {
                dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
            }
            int itemsCount = ignoreCount ? 0 : dbQuery.Count();
            if (!string.IsNullOrEmpty(queryInput.Sort))
            {
                dbQuery = dbQuery.OrderBy(queryInput.Sort);
            }
            if (queryInput.Page > 0 || queryInput.PageSize > 0)
            {
                int page = queryInput.Page > 0 ? queryInput.Page : 1;
                int pageSize = queryInput.PageSize > 0 ? queryInput.PageSize : 100;
                dbQuery = dbQuery.Page(page, pageSize);
            }
            return (dbQuery, itemsCount);
        }

        private IQueryable<TListModel> GetBaseQuery(Guid parentId)
        {
            IQueryable<TListModel> qry;
            if (string.IsNullOrEmpty(_filterByParentId))
            {
                qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
            else
            {
                if (parentId == null || parentId == Guid.Empty)
                {
                    throw new BadRequestException($"{nameof(parentId)} is required");
                }
                qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .Where(_filterByParentId, parentId.ToString())
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
        }
    }
}
