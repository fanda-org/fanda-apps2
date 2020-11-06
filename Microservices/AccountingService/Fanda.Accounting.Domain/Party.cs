using System;
using Fanda.Core;

namespace Fanda.Accounting.Domain
{
    public class Party
    {
        public Guid LedgerId { get; set; }
        public Guid PartyOrgId { get; set; }
        public Guid PartyTypeId { get; set; }
        public Guid CategoryId { get; set; }
        public PaymentTerm PaymentTerm { get; set; }

        public string PaymentTermString
        {
            get => PaymentTerm.ToString();
            set => PaymentTerm = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), value, true);
        }

        public decimal CreditLimit { get; set; }

        public virtual Organization PartyOrg { get; set; }
        public virtual PartyType Type { get; set; }
        public virtual PartyCategory Category { get; set; }
        public virtual Ledger Ledger { get; set; }
    }
}