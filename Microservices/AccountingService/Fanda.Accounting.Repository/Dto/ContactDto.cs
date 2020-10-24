using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Accounting.Repository.Dto
{
    public class ContactDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Salutation")]
        [StringLength(5, ErrorMessage = "Maximum allowed length is 5")]
        public string Salutation { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string LastName { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Designation { get; set; }

        [Display(Name = "Department")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Department { get; set; }

        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "Maximum allowed length is 100")]
        public string Email { get; set; }

        [Display(Name = "Work Phone")]
        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string WorkPhone { get; set; }

        [Display(Name = "Mobile")]
        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string Mobile { get; set; }

        [Display(Name = "Is Primary?")]
        public bool IsPrimary { get; set; }
    }
}
