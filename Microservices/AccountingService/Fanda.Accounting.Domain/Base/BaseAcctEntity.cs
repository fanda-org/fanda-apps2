using Fanda.Core.Base;
using System;

namespace Fanda.Accounting.Domain.Base
{
    public class OrgAcctEntity : BaseOrgEntity
    {
        public virtual Organization Organization { get; set; }
    }

    public class YearAcctEntity : BaseYearEntity
    {
        public string ReferenceNumber { get; set; }
        public DateTime ReferenceDate { get; set; }

        public virtual AccountYear AccountYear { get; set; }
    }
}
