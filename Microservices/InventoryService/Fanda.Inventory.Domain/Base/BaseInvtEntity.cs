using Fanda.Core.Base;

using System;

namespace Fanda.Inventory.Domain.Base
{
    public class OrgInvtEntity : BaseEntity
    {
        public Guid OrgId { get; set; }
        //public virtual Organization Organization { get; set; }
    }

    public class YearInvtEntity
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime ReferenceDate { get; set; }
        public Guid YearId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        //public virtual AccountYear AccountYear { get; set; }
    }
}