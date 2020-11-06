using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Repository.Dto
{
    public class OrgUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        public virtual List<OrgRoleDto> OrgRoles { get; set; }
    }

    public class OrgUserListDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }
}