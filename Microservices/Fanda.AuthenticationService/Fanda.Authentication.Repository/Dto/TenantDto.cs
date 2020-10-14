using Fanda.Core.Base;

namespace Fanda.Authentication.Repository.Dto
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
