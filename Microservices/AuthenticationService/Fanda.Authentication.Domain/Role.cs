using Fanda.Authentication.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Authentication.Domain
{
    public class Role : TenantEntity
    {
        public ICollection<RolePrivilege> Privileges { get; set; }
    }
}
