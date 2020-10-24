using Fanda.Accounting.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Domain
{
    public class Transaction : YearAcctEntity
    {
        public Guid DebitLedgerId { get; set; }
        public Guid CreditLedgerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public Ledger DebitLedger { get; set; }
        public Ledger CreditLedger { get; set; }
    }
}