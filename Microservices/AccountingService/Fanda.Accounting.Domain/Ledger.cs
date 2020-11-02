using Fanda.Accounting.Domain.Base;
using Fanda.Core;
using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class Ledger : OrgAcctEntity
    {
        public LedgerType LedgerType { get; set; }

        public string LedgerTypeString
        {
            get { return LedgerType.ToString(); }
            set { LedgerType = (LedgerType)Enum.Parse(typeof(LedgerType), value, true); }
        }

        public Guid LedgerGroupId { get; set; }
        public bool IsSystem { get; set; }

        public virtual LedgerGroup LedgerGroup { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<LedgerBalance> LedgerBalances { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<JournalItem> JournalItems { get; set; }
        public virtual ICollection<Transaction> DebitTransactions { get; set; }
        public virtual ICollection<Transaction> CreditTransactions { get; set; }
    }
}
