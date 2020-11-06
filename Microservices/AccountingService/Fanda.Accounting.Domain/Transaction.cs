using Fanda.Accounting.Domain.Base;
using System;

namespace Fanda.Accounting.Domain
{
    public class Transaction : YearAcctEntity
    {
        public Guid DebitLedgerId { get; set; }
        public Guid CreditLedgerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? JournalId { get; set; }

        public virtual Ledger DebitLedger { get; set; }
        public virtual Ledger CreditLedger { get; set; }
        public virtual Journal Journal { get; set; }
    }
}