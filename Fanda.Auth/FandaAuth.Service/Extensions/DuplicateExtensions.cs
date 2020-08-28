using Fanda.Core;
using Fanda.Core.Extensions;
using FandaAuth.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FandaAuth.Service.Extensions
{
    public class TenantKeyData : KeyData
    {
        public Guid TenantId { get; set; } //= default;
    }

    public class UserKeyData : KeyData
    {
        public Guid TenantId { get; set; } //= default;
    }

    public static class DuplicateExtensions
    {
        public static async Task<bool> ExistsAsync<TModel>(this DbContext context, TenantKeyData data)
            where TModel : TenantEntity
        {
            bool result = true;
            switch (data.Field)
            {
                case KeyField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>()
                            .AnyAsync(pc => pc.Id == data.Id);
                    }
                    return result;

                case KeyField.Code:
                    if (data.TenantId == null || data.TenantId == Guid.Empty)
                    {
                        throw new ArgumentNullException("tenantId", "Tenant Id is required");
                    }

                    if (data.Id == Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.TenantId == data.TenantId);
                    }
                    else if (data.Id != Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.TenantId == data.TenantId);
                    }
                    return result;

                case KeyField.Name:
                    if (data.TenantId == null || data.TenantId == Guid.Empty)
                    {
                        throw new ArgumentNullException("tenantId", "Tenant Id is required");
                    }

                    if (data.Id == Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.TenantId == data.TenantId);
                    }
                    else if (data.Id != Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.TenantId == data.TenantId);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<TModel> GetByAsync<TModel>(this DbContext context, TenantKeyData data)
            where TModel : TenantEntity
        {
            TModel result = default;
            switch (data.Field)
            {
                case KeyField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Id == data.Id);
                    }
                    return result;

                case KeyField.Code:
                    if (data.TenantId == null || data.TenantId == Guid.Empty)
                    {
                        throw new ArgumentNullException("tenantId", "Tenant Id is required");
                    }

                    if (data.Id == Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value && pc.TenantId == data.TenantId);
                    }
                    else if (data.Id != Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.TenantId == data.TenantId);
                    }
                    return result;

                case KeyField.Name:
                    if (data.TenantId == null || data.TenantId == Guid.Empty)
                    {
                        throw new ArgumentNullException("tenantId", "Tenant Id is required");
                    }

                    if (data.Id == Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value && pc.TenantId == data.TenantId);
                    }
                    else if (data.Id != Guid.Empty && data.TenantId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.TenantId == data.TenantId);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<bool> ExistsAsync<TModel>(this DbContext context, UserKeyData data)
            where TModel : UserEntity
        {
            bool result = true;
            switch (data.Field)
            {
                case KeyField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>()
                            .AnyAsync(pc => pc.Id == data.Id);
                    }
                    return result;

                case KeyField.Email:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Email == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Email == data.Value && pc.Id != data.Id);
                    }
                    return result;

                case KeyField.Name:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.UserName == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.UserName == data.Value && pc.Id != data.Id);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<bool> GetByAsync<TModel>(this DbContext context, UserKeyData data)
            where TModel : UserEntity
        {
            bool result = true;
            switch (data.Field)
            {
                case KeyField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>()
                            .AnyAsync(pc => pc.Id == data.Id);
                    }
                    return result;

                case KeyField.Email:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Email == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Email == data.Value && pc.Id != data.Id);
                    }
                    return result;

                case KeyField.Name:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.UserName == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.UserName == data.Value && pc.Id != data.Id);
                    }
                    return result;

                default:
                    return result;
            }
        }

        //public static async Task<bool> ExistsAsync<TModel>(this DbContext context, bool isEmailModel, ParentDuplicate data)
        //    where TModel : EmailEntity
        //{
        //    if (isEmailModel && data.Field == DuplicateField.Code)
        //    {
        //        throw new ArgumentException("Root should not have field 'code' for exist validation");
        //    }

        //    bool result = true;
        //    switch (data.Field)
        //    {
        //        case DuplicateField.Id:
        //            if (data.Id != Guid.Empty)
        //            {
        //                return await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Id == data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Email:
        //            if (data.Id == Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Email == data.Value);
        //            }
        //            else if (data.Id != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Email == data.Value && pc.Id != data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Name:
        //            if (data.Id == Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value);
        //            }
        //            else if (data.Id != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
        //            }
        //            return result;
        //        default:
        //            return true;
        //    }
        //}

        //public static async Task<bool> ExistsAsync<TModel>(this DbContext context, ParentDuplicate data)
        //    where TModel : BaseEntity
        //{
        //    bool result = true;
        //    switch (data.Field)
        //    {
        //        case DuplicateField.Id:
        //            if (data.Id != Guid.Empty)
        //            {
        //                return await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Id == data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Code:
        //            if (data.Id == Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value);
        //            }
        //            else if (data.Id != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Name:
        //            if (data.Id == Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value);
        //            }
        //            else if (data.Id != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
        //            }
        //            return result;
        //        default:
        //            return true;
        //    }
        //}

        //public static async Task<bool> ExistsAsync<TModel>(this DbContext context, Duplicate data)
        //    where TModel : BaseOrgEntity
        //{
        //    bool result = true;
        //    switch (data.Field)
        //    {
        //        case DuplicateField.Id:
        //            if (data.Id != Guid.Empty)
        //            {
        //                return await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Id == data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Code:
        //            if (data.ParentId == null || data.ParentId == Guid.Empty)
        //            {
        //                throw new ArgumentNullException("parentId", "Parent Id is required");
        //            }

        //            if (data.Id == Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value && pc.OrgId == data.ParentId);
        //            }
        //            else if (data.Id != Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.OrgId == data.ParentId);
        //            }
        //            return result;
        //        case DuplicateField.Name:
        //            if (data.ParentId == null || data.ParentId == Guid.Empty)
        //            {
        //                throw new ArgumentNullException("orgId", "Org Id is required");
        //            }

        //            if (data.Id == Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.OrgId == data.ParentId);
        //            }
        //            else if (data.Id != Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.OrgId == data.ParentId);
        //            }
        //            return result;
        //        default:
        //            return true;
        //    }
        //}

        //public static async Task<bool> ExistsAsync<TModel>(this AuthContext context, Duplicate data, bool isTenant)
        //    where TModel : BaseTenantEntity
        //{
        //    bool result = true;
        //    switch (data.Field)
        //    {
        //        case DuplicateField.Id:
        //            if (data.Id != Guid.Empty)
        //            {
        //                return await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Id == data.Id);
        //            }
        //            return result;
        //        case DuplicateField.Code:
        //            if (data.ParentId == null || data.ParentId == Guid.Empty)
        //            {
        //                throw new ArgumentNullException("parentId", "Parent Id is required");
        //            }

        //            if (data.Id == Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value && pc.TenantId == data.ParentId);
        //            }
        //            else if (data.Id != Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.TenantId == data.ParentId);
        //            }
        //            return result;
        //        case DuplicateField.Name:
        //            if (data.ParentId == null || data.ParentId == Guid.Empty)
        //            {
        //                throw new ArgumentNullException("orgId", "Org Id is required");
        //            }

        //            if (data.Id == Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.TenantId == data.ParentId);
        //            }
        //            else if (data.Id != Guid.Empty && data.ParentId != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.TenantId == data.ParentId);
        //            }
        //            return result;
        //        default:
        //            return true;
        //    }
        //}
    }
}
