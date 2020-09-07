using System;
using System.Threading.Tasks;

namespace Fanda.Core.Base
{
    public interface IRepositoryBase<TModel, TListModel, TKeyData> : IListRepositoryBase<TListModel>
    {
        // GET
        Task<TModel> GetByIdAsync(Guid id);

        // POST
        Task<TModel> CreateAsync(TModel model, Guid parentId);

        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // DELETE
        Task<bool> DeleteAsync(Guid id);

        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);

        // GET
        Task<bool> ExistsAsync(TKeyData data);

        // GET
        Task<TModel> GetByAsync(TKeyData data);

        // OPTIONS: INTERNAL
        Task<ValidationErrors> ValidateAsync(TModel model, Guid parentId);
    }
}
