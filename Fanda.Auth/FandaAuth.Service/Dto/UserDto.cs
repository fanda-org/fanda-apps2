using FandaAuth.Service.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace FandaAuth.Service.Dto
{
    public class UserDto : BaseUserDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public DateTime? DateLastLogin { get; set; }
    }

    public class UserListDto //: RootListDto
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool? Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
