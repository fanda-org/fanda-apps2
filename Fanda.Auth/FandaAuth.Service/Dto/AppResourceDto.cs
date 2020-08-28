using Fanda.Core;
using Fanda.Core.Base;

namespace FandaAuth.Service.Dto
{
    public class AppResourceDto : BaseDto
    {
        public ResourceType ResourceType { get; set; }
        //public Guid ApplicationId { get; set; }

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

    public class AppResourceListDto : BaseListDto
    {
        public ResourceType ResourceType { get; set; }
    }
}
