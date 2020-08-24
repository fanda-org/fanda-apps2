using Fanda.Core.Base;
using System;

namespace Fanda.Domain.Base
{
    public class OrgEntity : BaseEntity
    {
        public Guid OrgId { get; set; }
        public virtual Organization Organization { get; set; }
    }

    public class YearEntity
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guid YearId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual AccountYear AccountYear { get; set; }
    }
}
