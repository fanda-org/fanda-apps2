﻿using Fanda.Core.Base;
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

        public static async Task<DataResponse<IEnumerable<TListModel>>> GetAll<TEntity, TListModel>(
            this IListRepository<TEntity, TListModel> listRepository,
            Guid? parentId, Query queryInput)
        {
            if (queryInput.PageIndex > 0 || queryInput.PageSize > 0)
            {
                return await GetPaged(listRepository, parentId, queryInput);
            }

            return await GetList(listRepository, parentId, queryInput);
        }

        private static async Task<PagedResponse<IEnumerable<TListModel>>> GetPaged<TEntity, TListModel>(
            this IListRepository<TEntity, TListModel> listRepository,
            Guid? parentId, Query queryInput)
        {
            (var list, int itemsCount) = await listRepository.Execute(parentId, queryInput);
            var response =
                PagedResponse<IEnumerable<TListModel>>.Succeeded(list, itemsCount, queryInput.PageIndex,
                    queryInput.PageSize);
            return response;
        }

        private static async Task<DataResponse<IEnumerable<TListModel>>> GetList<TEntity, TListModel>(
            this IListRepository<TEntity, TListModel> listRepository,
            Guid? parentId, Query queryInput)
        {
            var (list, _) = await listRepository.Execute(parentId, queryInput, true);
            var response = DataResponse<IEnumerable<TListModel>>.Succeeded(list);
            return response;
        }

        private static async Task<(IEnumerable<TListModel>, int)> Execute<TEntity, TListModel>(
            this IListRepository<TEntity, TListModel> listRepository,
            Guid? parentId, Query queryInput, bool ignoreCount = false)
        {
            (var qry, int itemsCount) = listRepository.GenerateQuery(parentId, queryInput, ignoreCount);
            var list = await qry.ToListAsync();
            return (list, itemsCount);
        }

        private static (IQueryable<TListModel>, int) GenerateQuery<TEntity, TListModel>(
            this IListRepository<TEntity, TListModel> listRepository,
            Guid? parentId, Query queryInput, bool ignoreCount = false)
        {
            var dbQuery = listRepository.GetAll(parentId);
            if (!string.IsNullOrEmpty(queryInput.Filter))
            {
                dbQuery = dbQuery.Where(queryInput.Filter, queryInput.FilterArgs);
                // dbQuery = dbQuery.Where(c => EF.Functions.Like("name", "%fan%"));
            }

            int itemsCount = ignoreCount ? 0 : dbQuery.Count();
            if (!string.IsNullOrEmpty(queryInput.Sort))
            {
                dbQuery = dbQuery.OrderBy(queryInput.Sort);
            }

            if (queryInput.PageIndex > 0 || queryInput.PageSize > 0)
            {
                int pageIndex = queryInput.PageIndex > 0 ? queryInput.PageIndex : 1;
                int pageSize = queryInput.PageSize > 0 ? queryInput.PageSize : 100;
                dbQuery = dbQuery.Page(pageIndex, pageSize);
            }

            return (dbQuery, itemsCount);
        }
    }
}