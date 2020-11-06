using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;

namespace Fanda.Accounting.Service.Controllers
{
    public class LedgersController :
        SubController<ILedgerRepository, Ledger, LedgerDto, LedgerListDto>
    {
        public LedgersController(ILedgerRepository repository) : base(repository)
        {
        }
    }
}