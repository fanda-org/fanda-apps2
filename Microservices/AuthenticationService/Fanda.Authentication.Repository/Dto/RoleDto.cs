using System.Collections.Generic;
using Fanda.Core.Base;

namespace Fanda.Authentication.Repository.Dto
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