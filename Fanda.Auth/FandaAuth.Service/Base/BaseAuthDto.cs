using AutoMapper;
using Fanda.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FandaAuth.Service.Base
{
    public class BaseUserDto
    {
        public BaseUserDto()
        {
            Errors = new ValidationResultModel();
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
        public ValidationResultModel Errors { get; set; }

        public bool IsValid() => Errors.Count == 0;

        //public virtual TenantDto Tenant { get; set; }
    }
}
