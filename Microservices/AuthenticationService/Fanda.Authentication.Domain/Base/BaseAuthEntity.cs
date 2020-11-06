using System;
using Fanda.Core.Base;

namespace Fanda.Authentication.Domain.Base
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

    public class AppEntity : BaseEntity
    {
        public Guid ApplicationId { get; set; }
        public virtual Application Application { get; set; }
    }

    public class TenantEntity : BaseEntity
    {
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}