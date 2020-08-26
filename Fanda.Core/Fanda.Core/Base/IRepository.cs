using Fanda.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IParentRepositoryBase<TModel>
    {
        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // GET
        Task<TModel> GetByIdAsync(Guid id/*, bool includeChildren = false*/);

        // DELETE
        Task<bool> DeleteAsync(Guid id);

        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    public interface IRepositoryBase<TModel> : IParentRepositoryBase<TModel>
    {
        // POST
        Task<TModel> CreateAsync(Guid parentId, TModel model);

        // OPTOINS: INTERNAL
        Task<ValidationResultModel> ValidateAsync(Guid parentId, TModel model);
    }

    public interface IParentRepository<TModel> : IParentRepositoryBase<TModel>
    {
        // POST
        Task<TModel> CreateAsync(TModel model);

        // GET
        Task<bool> ExistsAsync(KeyData data);

        //GET
        Task<TModel> GetByAsync(KeyData data);

        // OPTIONS: INTERNAL
        Task<ValidationResultModel> ValidateAsync(TModel model);
    }

    //#region Get data of children

    //public interface IRepositoryChildData<TModel>
    //{
    //    // GET
    //    Task<TModel> GetChildrenByIdAsync(Guid parentId);
    //}

    //#endregion Get data of children

    #region List Repository

    public interface IListRepository<TModel>
    {
        // GET
        IQueryable<TModel> GetAll(Guid parentId);
    }

    #endregion List Repository
}
