using System.Collections.Generic;
using Fanda.Core.Base;

namespace Fanda.Authentication.Domain
{
    public class Application : BaseEntity
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public ICollection<AppResource> AppResources { get; set; }
    }
}