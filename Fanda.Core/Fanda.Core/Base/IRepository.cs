using Fanda.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IRootRepository<TModel>
    {
        // GET
        Task<TModel> GetByIdAsync(Guid id/*, bool includeChildren = false*/);

        // DELETE
        Task<bool> DeleteAsync(Guid id);

        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    public interface IListRepository<TListModel>
    {
        // GET
        IQueryable<TListModel> GetAll(Guid parentId);
    }

    public interface IParentRepository<TModel> : IRootRepository<TModel>
    {
        // POST
        Task<TModel> CreateAsync(TModel model);

        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // GET
        Task<bool> ExistsAsync(KeyData data);

        //GET
        Task<TModel> GetByAsync(KeyData data);

        // OPTIONS: INTERNAL
        Task<ValidationResultModel> ValidateAsync(TModel model);
    }

    public interface IRepository<TModel> : IRootRepository<TModel>
    {
        // POST
        Task<TModel> CreateAsync(Guid parentId, TModel model);

        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // OPTOINS: INTERNAL
        Task<ValidationResultModel> ValidateAsync(Guid parentId, TModel model);
    }

    //#region Get data of children

    //public interface IRepositoryChildData<TModel>
    //{
    //    // GET
    //    Task<TModel> GetChildrenByIdAsync(Guid parentId);
    //}

    //#endregion Get data of children
}
