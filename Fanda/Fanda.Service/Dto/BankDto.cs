using Fanda.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core.Models
{
    public class BankDto
    {
        public Guid LedgerId { get; set; }

        [Display(Name = "Account No.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No. is required")]
        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string AccountNumber { get; set; }

        [Display(Name = "Account Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account Type is required")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public BankAccountType AccountType { get; set; }

        [Display(Name = "IFSC")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string IfscCode { get; set; }

        [Display(Name = "MICR")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string MicrCode { get; set; }

        [Display(Name = "Branch Code")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string BranchCode { get; set; }

        [Display(Name = "Branch Name")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string BranchName { get; set; }

        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }

        public bool IsDefault { get; set; }

        //public bool IsDeleted { get; set; }
        //public int Index { get; set; }
    }
}