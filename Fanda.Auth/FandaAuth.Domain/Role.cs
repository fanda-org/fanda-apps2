using FandaAuth.Domain.Base;
using System.Collections.Generic;

namespace FandaAuth.Domain
{
    public class Role : TenantEntity
    {
        public ICollection<RolePrivilege> Privileges { get; set; }
    }
}
