using Fanda.Core.Base;

// using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Fanda.Core.Extensions
{
    public class KeyData
    {
        [Required]
        public KeyField Field { get; set; }

        [Required]
        public string Value { get; set; }

        public Guid Id { get; set; } //= default;
    }

    public static class DuplicateExtensions
    {
        //public static async Task<bool> ExistsAsync<TModel>(this DbContext context, ParentDuplicate data, bool isRoot)
        //    where TModel : RootEntity
        //{
        //    if (isRoot && data.Field == DuplicateField.Code)
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
        //                    .AnyAsync(pc => pc.Name == data.Value);
        //            }
        //            else if (data.Id != Guid.Empty)
        //            {
        //                result = await context.Set<TModel>()
        //                    .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
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

        public static async Task<bool> ExistsAsync<TModel>(this DbContext context, KeyData data)
            where TModel : BaseEntity
        {
            bool result = true;
            switch (data.Field)
            {
                case KeyField.Code:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id);
                    }
                    return result;

                case KeyField.Name:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static async Task<TModel> GetByAsync<TModel>(this DbContext context, KeyData data)
            where TModel : BaseEntity
        {
            TModel result = default;
            switch (data.Field)
            {
                case KeyField.Code:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Code == data.Value && pc.Id != data.Id);
                    }
                    return result;

                case KeyField.Name:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .FirstOrDefaultAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
                    }
                    return result;

                default:
                    return result;
            }
        }

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
        //                throw new ArgumentNullException("parentId", "Parent Id is missing");
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
        //                throw new ArgumentNullException("orgId", "Org Id is missing");
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
    }
}
