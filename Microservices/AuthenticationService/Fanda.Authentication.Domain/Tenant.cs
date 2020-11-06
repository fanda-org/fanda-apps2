using Fanda.Core.Base;
using System.Collections.Generic;

namespace Fanda.Authentication.Domain
{
    public class Tenant : BaseEntity
    {
        public int OrgCount { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}