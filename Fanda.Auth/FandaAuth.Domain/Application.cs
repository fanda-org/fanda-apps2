using Fanda.Core.Base;
using System.Collections.Generic;

namespace FandaAuth.Domain
{
    public class Application : BaseEntity
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public ICollection<AppResource> AppResources { get; set; }
    }
}
