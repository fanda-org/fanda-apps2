using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;

namespace Fanda.Accounting.Service.Controllers
{
    public class AccountYearsController :
        SubController<IAccountYearRepository, AccountYear, AccountYearDto, AccountYearListDto>
    {
        public AccountYearsController(IAccountYearRepository repository) : base(repository)
        {
        }
    }
}
