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
    public abstract class ListRepository<TEntity, TListModel> : IListRepository<TListModel>
        where TEntity : class
        where TListModel : class
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly string _filterBySuperId;

        public ListRepository(DbContext context, IMapper mapper, string filterBySuperId)
        {
            _context = context;
            _mapper = mapper;
            _filterBySuperId = filterBySuperId;
        }

        public virtual async Task<DataResponse<IEnumerable<TListModel>>> GetAll(Guid superId, Query queryInput)  // nullable
        {
            if (queryInput.Page > 0 || queryInput.PageSize > 0)
            {
                return await GetPaged(superId, queryInput);
            }
            else
            {
                return await GetList(superId, queryInput);
            }
        }

        private async Task<PagedResponse<IEnumerable<TListModel>>> GetPaged(Guid superId, Query queryInput)
        {
            var (list, itemsCount) = await Execute(superId, queryInput);
            var response = PagedResponse<IEnumerable<TListModel>>.Succeeded(list, itemsCount, queryInput.Page, queryInput.PageSize);
            return response;
        }

        private async Task<DataResponse<IEnumerable<TListModel>>> GetList(Guid superId, Query queryInput)
        {
            var (list, _) = await Execute(superId, queryInput, true);
            var response = DataResponse<IEnumerable<TListModel>>.Succeeded(list);
            return response;
        }

        private async Task<(IEnumerable<TListModel>, int)> Execute(Guid superId, Query queryInput, bool ignoreCount = false)
        {
            var (qry, itemsCount) = GenerateQuery(superId, queryInput, ignoreCount);
            var list = await qry.ToListAsync();
            return (list, itemsCount);
        }

        private (IQueryable<TListModel>, int) GenerateQuery(Guid superId, Query queryInput, bool ignoreCount = false)
        {
            var dbQuery = GetBaseQuery(superId);
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

        private IQueryable<TListModel> GetBaseQuery(Guid superId)
        {
            IQueryable<TListModel> qry;
            if (string.IsNullOrEmpty(_filterBySuperId))
            {
                qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
            else
            {
                if (superId == null || superId == Guid.Empty)
                {
                    throw new BadRequestException($"{nameof(superId)} is required");
                }
                qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .Where(_filterBySuperId, superId.ToString())
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }
        }
    }
}
