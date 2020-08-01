using Fanda.Core;
using Fanda.Core.Base;
using System;

namespace FandaAuth.Service.Dto
{
    public class AppResourceDto : BaseDto
    {
        public ResourceType ResourceType { get; set; }
        public Guid ApplicationId { get; set; }

        #region Action fields

        public bool Creatable { get; set; }
        public bool Updateable { get; set; }
        public bool Deleteable { get; set; }
        public bool Readable { get; set; }
        public bool Printable { get; set; }
        public bool Importable { get; set; }
        public bool Exportable { get; set; }

        #endregion Action fields
    }

    public class AppResourceListDto : BaseListDto
    {
        public ResourceType ResourceType { get; set; }
    }
}
