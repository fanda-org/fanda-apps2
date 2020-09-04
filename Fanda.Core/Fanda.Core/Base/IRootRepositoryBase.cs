﻿using System;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IRootRepositoryBase<TModel, TListModel> : IListRepositoryBase<TListModel>
    {
        // GET
        Task<TModel> GetByIdAsync(Guid id/*, bool includeChildren = false*/);

        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // DELETE
        Task<bool> DeleteAsync(Guid id);

        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    //public interface IRepository<TModel, TListModel> : IRootRepository<TModel, TListModel>
    //{
    //    // POST
    //    Task<TModel> CreateAsync(Guid parentId, TModel model);

    //    // PUT
    //    Task UpdateAsync(Guid id, TModel model);

    //    // OPTOINS: INTERNAL
    //    Task<ValidationResultModel> ValidateAsync(Guid parentId, TModel model);
    //}

    //#region Get data of children

    //public interface IRepositoryChildData<TModel>
    //{
    //    // GET
    //    Task<TModel> GetChildrenByIdAsync(Guid parentId);
    //}

    //#endregion Get data of children
}
