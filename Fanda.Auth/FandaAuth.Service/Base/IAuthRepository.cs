﻿using Fanda.Core.Base;
using FandaAuth.Service.Extensions;
using System.Threading.Tasks;

namespace FandaAuth.Service.Base
{
    public interface ITenantRepository<TModel, TListModel> : IRepository<TModel, TListModel>
    {
        // GET
        Task<bool> ExistsAsync(TenantKeyData data);

        //GET
        Task<TModel> GetByAsync(TenantKeyData data);
    }

    public interface IUserRepository<TModel, TListModel> : IRepository<TModel, TListModel>
    {
        // GET
        Task<bool> ExistsAsync(UserKeyData data);

        //GET
        Task<TModel> GetByAsync(UserKeyData data);
    }
}
