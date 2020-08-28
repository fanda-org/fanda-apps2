using Fanda.Core.Base;
using System.Collections.Generic;

namespace FandaAuth.Service.Dto
{
    //public class RoleBaseDto : BaseDto
    //{
    //}

    public class RoleDto : BaseDto
    {
        public List<RolePrivilegeDto> Privileges { get; set; }
    }

    public class RoleListDto : BaseListDto
    {
    }

    //public class RoleChildrenDto
    //{
    //    public List<RolePrivilegeDto> Privileges { get; set; }
    //}
}
