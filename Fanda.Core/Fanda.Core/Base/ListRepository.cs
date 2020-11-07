using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Fanda.Core.Base
{
    public interface IListRepository<TEntity, TListModel>
    {
        IQueryable<TListModel> GetAll(Guid? superId);

        Expression<Func<TEntity, bool>> GetSuperIdPredicate(Guid? superId);
    }

    public abstract class ListRepository<TEntity, TListModel> : IListRepository<TEntity, TListModel>
        where TEntity : class
        where TListModel : class
    {
        private readonly DbContext _context;

        // private readonly Expression<Func<TEntity, bool>> _filterBySuperId;
        private readonly IMapper _mapper;

        public ListRepository(DbContext context, IMapper mapper/*, Expression<Func<TEntity, bool>> filterBySuperId*/)
        {
            _context = context;
            _mapper = mapper;
            // _filterBySuperId = filterBySuperId;
        }

        //public virtual async Task<DataResponse<IEnumerable<TListModel>>> GetAll(Guid superId, Query queryInput)  // nullable
        //{
        //    if (queryInput.Page > 0 || queryInput.PageSize > 0)
        //    {
        //        return await GetPaged(superId, queryInput);
        //    }
        //    else
        //    {
        //        return await GetList(superId, queryInput);
        //    }
        //}

        //private async Task<PagedResponse<IEnumerable<TListModel>>> GetPaged(Guid superId, Query queryInput)
        //{
        //    var (list, itemsCount) = await Execute(superId, queryInput);
        //    var response = PagedResponse<IEnumerable<TListModel>>.Succeeded(list, itemsCount, queryInput.Page, queryInput.PageSize);
        //    return response;
        //}

        //private async Task<DataResponse<IEnumerable<TListModel>>> GetList(Guid superId, Query queryInput)
        //{
        //    var (list, _) = await Execute(superId, queryInput, true);
        //    var response = DataResponse<IEnumerable<TListModel>>.Succeeded(list);
        //    return response;
        //}

        //private async Task<(IEnumerable<TListModel>, int)> Execute(Guid superId, Query queryInput, bool ignoreCount = false)
        //{
        //    var (qry, itemsCount) = GenerateQuery(superId, queryInput, ignoreCount);
        //    var list = await qry.ToListAsync();
        //    return (list, itemsCount);
        //}

        //private (IQueryable<TListModel>, int) GenerateQuery(Guid superId, Query queryInput, bool ignoreCount = false)
        //{
        //    var dbQuery = GetBaseQuery(superId);
        //    if (!string.IsNullOrEmpty(queryInput.Filter))
        //    {
        //        dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
        //    }
        //    int itemsCount = ignoreCount ? 0 : dbQuery.Count();
        //    if (!string.IsNullOrEmpty(queryInput.Sort))
        //    {
        //        dbQuery = dbQuery.OrderBy(queryInput.Sort);
        //    }
        //    if (queryInput.Page > 0 || queryInput.PageSize > 0)
        //    {
        //        int page = queryInput.Page > 0 ? queryInput.Page : 1;
        //        int pageSize = queryInput.PageSize > 0 ? queryInput.PageSize : 100;
        //        dbQuery = dbQuery.Page(page, pageSize);
        //    }
        //    return (dbQuery, itemsCount);
        //}

        public virtual IQueryable<TListModel> GetAll(Guid? superId)
        {
            IQueryable<TListModel> qry;
            if (superId == null || superId == Guid.Empty /*string.IsNullOrEmpty(_filterBySuperId)*/)
            {
                qry = _context.Set<TEntity>()
                    .AsNoTracking()
                    .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
                return qry;
            }

            //if (superId == Guid.Empty)
            //{
            //    throw new BadRequestException($"{nameof(superId)} is required");
            //}

            qry = _context.Set<TEntity>()
                .AsNoTracking()
                .Where(GetSuperIdPredicate(superId))
                .ProjectTo<TListModel>(_mapper.ConfigurationProvider);

            return qry;
        }

        public abstract Expression<Func<TEntity, bool>> GetSuperIdPredicate(Guid? superId);
    }
}