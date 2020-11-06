using System;
using System.Collections.Generic;
using Fanda.Accounting.Domain.Base;

namespace Fanda.Accounting.Domain
{
    public class AccountYear : OrgAcctEntity
    {
        //public Guid Id { get; set; }
        //public string YearCode { get; set; }
        public DateTime YearBegin { get; set; }

        public DateTime YearEnd { get; set; }
        //public Guid OrgId { get; set; }

        //public virtual Organization Organization { get; set; }
        //public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual ICollection<LedgerBalance> LedgerBalances { get; set; }
        public virtual ICollection<SerialNumber> SerialNumbers { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}