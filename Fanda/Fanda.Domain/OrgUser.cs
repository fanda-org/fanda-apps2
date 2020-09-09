using System;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class OrgUser
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<OrgUserRole> OrgUserRoles { get; set; }
    }
}
