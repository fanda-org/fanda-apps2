using Fanda.Core;
using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class Party
    {
        public Guid LedgerId { get; set; }
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }
        public PartyType PartyType { get; set; }

        public string PartyTypeString
        {
            get { return PartyType.ToString(); }
            set { PartyType = (PartyType)Enum.Parse(typeof(PartyType), value, true); }
        }

        public PaymentTerm PaymentTerm { get; set; }

        public string PaymentTermString
        {
            get { return PaymentTerm.ToString(); }
            set { PaymentTerm = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), value, true); }
        }

        public decimal CreditLimit { get; set; }
        public Guid CategoryId { get; set; }

        public virtual PartyCategory Category { get; set; }
        public virtual Ledger Ledger { get; set; }
        public virtual ICollection<PartyContact> PartyContacts { get; set; }
        public virtual ICollection<PartyAddress> PartyAddresses { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
