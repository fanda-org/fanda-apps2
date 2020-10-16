using AutoMapper;
using Fanda.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fanda.Authentication.Repository.Base
{
    public class BaseUserDto
    {
        public BaseUserDto()
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

        //[Required]
        //[Display(Name = "Tenant ID")]
        //public Guid TenantId { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [JsonIgnore(), IgnoreDataMember(), IgnoreMap()]
        public ValidationErrors Errors { get; set; }

        public bool IsValid() => Errors.Count == 0;

        //public virtual TenantDto Tenant { get; set; }
    }
}
