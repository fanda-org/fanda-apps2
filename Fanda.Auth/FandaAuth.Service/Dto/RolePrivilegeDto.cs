using System;
using System.ComponentModel.DataAnnotations;

namespace FandaAuth.Service.Dto
{
    public class RolePrivilegeDto
    {
        // Role
        //[Required]
        //public Guid RoleId { get; set; }

        // AppResource
        [Required]
        public Guid AppResourceId { get; set; }

        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Read { get; set; }
        public bool Print { get; set; }
        public bool Import { get; set; }
        public bool Export { get; set; }
    }
}
