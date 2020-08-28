using Fanda.Core.Base;

namespace FandaAuth.Service.Base
{
    public abstract class TenantControllerBase<TRepository, TModel, TListModel> :
        RootControllerBase<TRepository, TModel, TListModel>
        where TRepository : ITenantRepository<TModel>, IListRepository<TListModel>
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        public TenantControllerBase(TRepository repository, string moduleName)
            : base(repository, moduleName)
        {
        }
    }
}
