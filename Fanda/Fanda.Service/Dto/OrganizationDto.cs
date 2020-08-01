using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core.Models
{
    public class OrganizationDto : BaseDto
    {
        [Display(Name = "Regd.No.", Prompt = "Registered number"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string RegdNum { get; set; }

        [Display(Name = "PAN", Prompt = "Permanent account number"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string PAN { get; set; }

        [Display(Name = "TAN", Prompt = "Tax deduction and collection account number"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string TAN { get; set; }

        [Display(Name = "GSTIN", Prompt = "Goods and services tax identification number"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string GSTIN { get; set; }

        public virtual ICollection<ContactDto> Contacts { get; set; }
        public virtual ICollection<AddressDto> Addresses { get; set; }
    }

    public class OrgListDto : BaseListDto { }

    public class OrgYearListDto : BaseListDto
    {
        public bool IsSelected { get; set; }
        public Guid SelectedYearId { get; set; }

        [Display(Name = "Accounting Years")]
        public ICollection<YearListDto> AccountYears { get; set; }
    }

    public class OrgChildrenDto
    {
        public ICollection<ContactDto> Contacts { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
    }
}