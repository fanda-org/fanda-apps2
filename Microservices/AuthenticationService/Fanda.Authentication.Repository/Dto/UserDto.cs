using AutoMapper;
using Fanda.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fanda.Authentication.Repository.Dto
{
    public class UserDto //: BaseUserDto
    {
        public UserDto()
        {
            Errors = new ValidationErrors();
        }

        public Guid Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public bool? ResetPassword { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [JsonIgnore(), IgnoreDataMember(), IgnoreMap()]
        public ValidationErrors Errors { get; set; }

        public bool IsValid() => Errors.Count == 0;
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
