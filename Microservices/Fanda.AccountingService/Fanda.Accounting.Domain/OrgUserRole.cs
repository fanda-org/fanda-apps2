using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class OrgUserRole
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual ICollection<OrgUser> OrgUsers { get; set; }
    }
}
