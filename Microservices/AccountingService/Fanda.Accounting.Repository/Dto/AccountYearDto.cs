using Fanda.Core.Base;
using System;

namespace Fanda.Accounting.Repository.Dto
{
    public class AccountYearDto : BaseDto
    {
        public DateTime YearBegin { get; set; }
        public DateTime YearEnd { get; set; }
    }

    public class YearListDto : BaseListDto
    {
    }
}
