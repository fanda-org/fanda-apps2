using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Service.Extensions;
using System;
using System.Threading.Tasks;

namespace FandaAuth.Service.Base
{
    public interface ITenantRepositoryBase<TModel, TListModel> : IRootRepositoryBase<TModel, TListModel>
    {
        // POST
        Task<TModel> CreateAsync(Guid tenantId, TModel model);

        // PUT
        Task UpdateAsync(Guid id, TModel model);

        // OPTOINS: INTERNAL
        Task<ValidationErrors> ValidateAsync(Guid tenantId, TModel model);

        // GET
        Task<bool> ExistsAsync(TenantKeyData data);

        //GET
        Task<TModel> GetByAsync(TenantKeyData data);
    }
}
