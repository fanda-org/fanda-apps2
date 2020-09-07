using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IListRepositoryBase<TListModel>
    {
        // GET
        Task<DataResponse<IEnumerable<TListModel>>> GetAll(Guid parentId, Query queryInput);
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
