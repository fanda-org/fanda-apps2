using Fanda.Core.Base;
using System.Collections.Generic;

namespace FandaAuth.Domain
{
    public class Tenant : BaseEntity
    {
        public int OrgCount { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
