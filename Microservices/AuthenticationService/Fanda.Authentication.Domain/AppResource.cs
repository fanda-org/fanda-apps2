using Fanda.Authentication.Domain.Base;
using Fanda.Core;
using System;
using System.Collections.Generic;

namespace Fanda.Authentication.Domain
{
    public class AppResource : AppEntity
    {
        public ResourceType ResourceType { get; set; }

        public string ResourceTypeString
        {
            get => ResourceType.ToString();
            set => ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true);
        }

        public virtual ICollection<RolePrivilege> Privileges { get; set; }

        #region Action fields

        public bool Creatable { get; set; }
        public bool Updatable { get; set; }
        public bool Deletable { get; set; }
        public bool Readable { get; set; }
        public bool Printable { get; set; }
        public bool Importable { get; set; }
        public bool Exportable { get; set; }

        #endregion Action fields
    }
}