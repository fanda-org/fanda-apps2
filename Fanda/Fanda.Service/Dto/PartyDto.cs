﻿using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core.Models
{
    public class PartyDto
    {
        public PartyDto()
        {
            Contacts = new HashSet<ContactDto>();
            Addresses = new HashSet<AddressDto>();
        }
        public Guid LedgerId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select contact category")]
        [Display(Name = "Contact Category")]
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Regn.No.")]
        public string RegdNum { get; set; }

        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        [Display(Name = "Contact Type")]
        public PartyType PartyType { get; set; }

        [Display(Name = "Payment Term")]
        public PaymentTerm PaymentTerm { get; set; }

        [Display(Name = "Credit Limit")]
        public decimal CreditLimit { get; set; }

        public ICollection<ContactDto> Contacts { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
    }
}