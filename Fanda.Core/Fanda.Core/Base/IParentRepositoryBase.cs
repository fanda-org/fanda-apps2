using Fanda.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IParentRepositoryBase<TModel, TListModel> : IRootRepositoryBase<TModel, TListModel>
    {
        // POST
        Task<TModel> CreateAsync(TModel model);

        // GET
        Task<bool> ExistsAsync(KeyData data);

        //GET
        //Task<TModel> GetByAsync(KeyData data);
        // OPTIONS: INTERNAL
        // Task<ValidationResultModel> ValidateAsync(TModel model);
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
