using Fanda.Core;
using FandaAuth.Domain.Base;
using System;
using System.Collections.Generic;

namespace FandaAuth.Domain
{
    public class AppResource : AppEntity
    {
        public ResourceType ResourceType { get; set; }

        public string ResourceTypeString
        {
            get { return ResourceType.ToString(); }
            set { ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true); }
        }

        #region Action fields

        public bool Creatable { get; set; }
        public bool Updateable { get; set; }
        public bool Deleteable { get; set; }
        public bool Readable { get; set; }
        public bool Printable { get; set; }
        public bool Importable { get; set; }
        public bool Exportable { get; set; }

        #endregion Action fields

        public virtual ICollection<RolePrivilege> Privileges { get; set; }
    }
}
