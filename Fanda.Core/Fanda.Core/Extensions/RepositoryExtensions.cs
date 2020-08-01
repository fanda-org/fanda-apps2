using Fanda.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Core.Extensions
{
    public static class RepositoryExtensions
    {
        //public static Task<PagedList<T>> GetAll<T>(this IRepositoryList<T> repositoryList, Query queryInput)
        //    where T: class
        //{
        //    var dbQuery = GetQueryable(repositoryList.GetAll(), queryInput);      //.AsQueryable();
        //    return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        //}

        //public static Task<PagedList> GetAll(this IRepositoryChildList repositoryList, Guid parentId, Query queryInput)
        //{
        //    var dbQuery = GetQueryable(repositoryList.GetAll(parentId), queryInput);      //.AsQueryable();
        //    return dbQuery.GetPagedAsync(); //ToDynamicListAsync();
        //}

        public static async Task<DataResponse<IEnumerable<TModel>>> GetData<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            if (queryInput.Page > 0 || queryInput.PageSize > 0)
            {
                return await GetPaged(listRepository, parentId, queryInput);
            }
            else
            {
                return await GetList(listRepository, parentId, queryInput);
            }
        }

        private static async Task<DataResponse<IEnumerable<TModel>>> GetList<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var (list, _) = await listRepository.GetModelList(parentId, queryInput);
            var response = DataResponse<IEnumerable<TModel>>.Succeeded(list);
            return response;
        }

        private static async Task<PagedResponse<IEnumerable<TModel>>> GetPaged<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var (list, itemsCount) = await listRepository.GetModelList(parentId, queryInput);
            var response = PagedResponse<IEnumerable<TModel>>.Succeeded(list, itemsCount, queryInput.Page, queryInput.PageSize);
            return response;
        }

        private static async Task<(IEnumerable<TModel>, int)> GetModelList<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var (qry, itemsCount) = listRepository.GetModelQuery(parentId, queryInput);
            var list = await qry.ToListAsync();
            return (list, itemsCount);
        }

        private static (IQueryable<TModel>, int) GetModelQuery<TModel>(this IListRepository<TModel> listRepository,
            Guid parentId, Query queryInput)
        {
            var dbQuery = listRepository.GetAll(parentId);
            if (!string.IsNullOrEmpty(queryInput.Filter))
            {
                dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
            }
            int itemsCount = dbQuery.Count();
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
    }
}
