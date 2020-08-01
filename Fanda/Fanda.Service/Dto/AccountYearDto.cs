using Fanda.Shared;
using System;

namespace Fanda.Core.Models
{
    public class AccountYearDto : BaseDto
    {
        //public Guid Id { get; set; }
        //public string YearCode { get; set; }
        public DateTime YearBegin { get; set; }
        public DateTime YearEnd { get; set; }
    }

    public class YearListDto : BaseListDto
    {
    }
}
