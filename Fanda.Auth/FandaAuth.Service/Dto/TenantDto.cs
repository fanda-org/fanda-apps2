using Fanda.Core.Base;

namespace FandaAuth.Service.Dto
{
    public class TenantDto : BaseDto
    {
        public int OrgCount { get; set; }
    }

    public class TenantListDto : BaseListDto
    {
        public int OrgCount { get; set; }
    }
}
