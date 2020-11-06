using Fanda.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository.Helpers
{
    public static class DbUpdateHelper
    {
        public static async Task<int> UpdateAsync<T>(DbContext dbContext,
            T entity, params Expression<Func<T, object>>[] navigation)
            where T : BaseEntity
        {
            var dbEntity = await dbContext.FindAsync<T>(entity.Id);

            var dbEntry = dbContext.Entry(dbEntity);
            dbEntry.CurrentValues.SetValues(entity);

            foreach (var property in navigation)
            {
                string propertyName = property.GetPropertyAccess().Name;
                var dbItemsEntry = dbEntry.Collection(propertyName);
                var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                await dbItemsEntry.LoadAsync();
                var dbItemsMap = ((IEnumerable<BaseEntity>)dbItemsEntry.CurrentValue)
                    .ToDictionary(e => e.Id);

                var items = (IEnumerable<BaseEntity>)accessor.GetOrCreate(entity, false);

                foreach (var item in items)
                {
                    if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                    {
                        accessor.Add(dbEntity, item, false);
                    }
                    else
                    {
                        dbContext.Entry(oldItem).CurrentValues.SetValues(item);
                        dbItemsMap.Remove(item.Id);
                    }
                }

                foreach (var oldItem in dbItemsMap.Values)
                {
                    accessor.Remove(dbEntity, oldItem);
                }
            }

            return await dbContext.SaveChangesAsync();
        }
    }
}