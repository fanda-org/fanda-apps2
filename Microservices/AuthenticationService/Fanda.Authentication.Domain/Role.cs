using System.Collections.Generic;
using Fanda.Authentication.Domain.Base;

namespace Fanda.Authentication.Domain
{
    public class Role : TenantEntity
    {
        public ICollection<RolePrivilege> Privileges { get; set; }
    }
}