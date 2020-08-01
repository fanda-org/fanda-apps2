using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IRepositoryBase<TModel>
    {
        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // GET
        Task<TModel> GetByIdAsync(Guid id, bool includeChildren = false);

        // DELETE
        Task<bool> DeleteAsync(Guid id);

        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    public interface IParentRepository<TModel> : IRepositoryBase<TModel>
    {
        // POST
        Task<TModel> CreateAsync(TModel model);

        // GET
        Task<bool> ExistsAsync(ParentDuplicate data);

        Task<ValidationResultModel> ValidateAsync(TModel model);
    }

    public interface IRepository<TModel> : IRepositoryBase<TModel>
    {
        // POST
        Task<TModel> CreateAsync(Guid parentId, TModel model);

        // GET
        Task<bool> ExistsAsync(Duplicate data);

        Task<ValidationResultModel> ValidateAsync(Guid parentId, TModel model);
    }

    #region Get data of children

    public interface IRepositoryChildData<TModel>
    {
        // GET
        Task<TModel> GetChildrenByIdAsync(Guid parentId);
    }

    #endregion Get data of children

    #region List Repository

    public interface IListRepository<TModel>
    {
        // GET
        IQueryable<TModel> GetAll(Guid parentId);
    }

    #endregion List Repository
}
