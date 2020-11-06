using Fanda.Core.Base;
using System;

namespace Fanda.Inventory.Domain.Base
{
    public class OrgInvtEntity : BaseOrgEntity
    {
    }

    public class YearInvtEntity : BaseYearEntity
    {
        public string ReferenceNumber { get; set; }

        public DateTime ReferenceDate { get; set; }
        //public Guid YearId { get; set; }
    }
}