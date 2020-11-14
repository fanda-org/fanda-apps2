using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fanda.Core.Base
{
    //public class RootDto
    //{
    //    public RootDto()
    //    {
    //        Errors = new ValidationResultModel();
    //    }

    //    [Required]
    //    public Guid Id { get; set; }

    //    [Display(Name = "Name"),
    //        Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required"),
    //        //RegularExpression(@"^[a-zA-Z0-9\s~!@#$%^&*()_+-=\\|{}\[\];:',.<>/?\/]*$", ErrorMessage = @"{0} must not contain special characters"),
    //        StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
    //    public string Name { get; set; }

    //    public bool Active { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime? DateModified { get; set; }

    //    [JsonIgnore(), IgnoreDataMember(), IgnoreMap()]
    //    public ValidationResultModel Errors { get; set; }
    //    public bool IsValid() => Errors.Count == 0;
    //}

    public class BaseDto //: RootDto
    {
        public BaseDto()
        {
            Errors = new ValidationErrors();
        }

        [Required] public Guid Id { get; set; }

        [Display(Name = "Code", Prompt = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "{0} cannot exceeed {2} characters")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Description { get; set; }

        public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreMap]
        public ValidationErrors Errors { get; set; }

        public bool IsValid()
        {
            return Errors.Count == 0;
        }
    }

    //public class RootListDto
    //{
    //    [Required]
    //    public Guid Id { get; set; }

    //    [Display(Name = "Name")]
    //    public string Name { get; set; }

    //    public bool? Active { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime? DateModified { get; set; }
    //}

    public class BaseListDto //: RootListDto
    {
        [Required] public Guid Id { get; set; }

        [Display(Name = "Code")] public string Code { get; set; }

        [Display(Name = "Name")] public string Name { get; set; }

        [Display(Name = "Description")] public string Description { get; set; }

        public bool? Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }
    }

    public class BaseYearDto
    {
        public BaseYearDto()
        {
            Errors = new ValidationErrors();
        }

        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [IgnoreMap]
        public ValidationErrors Errors { get; set; }

        public bool IsValid()
        {
            return Errors.Count == 0;
        }
    }

    public class BaseYearListDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string LedgerName { get; set; }
        public decimal NetAmount { get; set; }
    }
}