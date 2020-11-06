using System.Collections.Generic;
using Fanda.Core.Base;

namespace Fanda.Authentication.Domain
{
    public class Tenant : BaseEntity
    {
        public int OrgCount { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}