using Fanda.Core.Base;
using System.Collections.Generic;

namespace FandaAuth.Service.Dto
{
    public class ApplicationDto : BaseDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public List<AppResourceDto> AppResources { get; set; }
    }

    public class ApplicationListDto : BaseListDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }
    }

    public class AppChildrenDto
    {
        public List<AppResourceDto> AppResources { get; set; }
    }
}
