using Fanda.Core;
using Fanda.Core.Extensions;
using Fanda.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Fanda.Service.Extensions
{
    public class OrgKeyData : KeyData
    {
        public Guid OrgId { get; set; } //= default;
    }

    public class YearKeyData : KeyData
    {
        public Guid YearId { get; set; } //= default;
    }

    public static class DuplicateExtensions
    {
        public static async Task<bool> ExistsAsync<TModel>(this DbContext context, OrgKeyData data)
            where TModel : OrgEntity
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
                    if (data.OrgId == null || data.OrgId == Guid.Empty)
                    {
                        throw new ArgumentNullException("orgId", "Org Id is required");
                    }

                    if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;

                case KeyField.Name:
                    if (data.OrgId == null || data.OrgId == Guid.Empty)
                    {
                        throw new ArgumentNullException("orgId", "Org Id is required");
                    }

                    if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<TModel> GetByAsync<TModel>(this DbContext context, OrgKeyData data)
            where TModel : OrgEntity
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
                    if (data.OrgId == null || data.OrgId == Guid.Empty)
                    {
                        throw new ArgumentNullException("orgId", "Org Id is required");
                    }

                    if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;

                case KeyField.Name:
                    if (data.OrgId == null || data.OrgId == Guid.Empty)
                    {
                        throw new ArgumentNullException("orgId", "Org Id is required");
                    }

                    if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<bool> ExistsAsync<TModel>(this DbContext context, YearKeyData data)
            where TModel : YearEntity
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

                case KeyField.Number:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Number == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Number == data.Value && pc.Id != data.Id);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<bool> GetByAsync<TModel>(this DbContext context, YearKeyData data)
            where TModel : YearEntity
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

                case KeyField.Number:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Number == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Number == data.Value && pc.Id != data.Id);
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
