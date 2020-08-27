using Fanda.Core.Base;
using Fanda.Service.Extensions;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public interface IOrgRepository<TModel> : IRepositoryBase<TModel>
    {
        // GET
        Task<bool> ExistsAsync(OrgKeyData data);

        //GET
        Task<TModel> GetByAsync(OrgKeyData data);
    }

    public interface IYearRepository<TModel> : IRepositoryBase<TModel>
    {
        // GET
        Task<bool> ExistsAsync(YearKeyData data);

        //GET
        Task<TModel> GetByAsync(YearKeyData data);
    }
}
