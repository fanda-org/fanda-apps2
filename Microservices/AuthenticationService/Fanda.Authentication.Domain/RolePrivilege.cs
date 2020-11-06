using System;

namespace Fanda.Authentication.Domain
{
    public class RolePrivilege
    {
        // Role
        public Guid RoleId { get; set; }

        // AppResource
        public Guid AppResourceId { get; set; }

        #region Actions

        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Read { get; set; }
        public bool Print { get; set; }
        public bool Import { get; set; }
        public bool Export { get; set; }

        #endregion Actions

        #region Virtual members

        public virtual Role Role { get; set; }
        public virtual AppResource AppResource { get; set; }

        #endregion Virtual members
    }
}