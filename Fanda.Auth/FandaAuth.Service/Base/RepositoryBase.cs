using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FandaAuth.Service.Base
{
    public interface ITenantRepository<TModel> : IRepositoryBase<TModel>
    {
        // GET
        Task<bool> ExistsAsync(TenantKeyData data);

        //GET
        Task<TModel> GetByAsync(TenantKeyData data);
    }

    public interface IUserRepository<TModel> : IRepositoryBase<TModel>
    {
        // GET
        Task<bool> ExistsAsync(UserKeyData data);

        //GET
        Task<TModel> GetByAsync(UserKeyData data);
    }
}
