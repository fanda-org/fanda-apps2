using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;

namespace Fanda.Accounting.Service.Controllers
{
    public class LedgerGroupsController :
        SubController<ILedgerGroupRepository, LedgerGroup, LedgerGroupDto, LedgerGroupListDto>
    {
        public LedgerGroupsController(ILedgerGroupRepository repository) : base(repository)
        {
        }
    }
}