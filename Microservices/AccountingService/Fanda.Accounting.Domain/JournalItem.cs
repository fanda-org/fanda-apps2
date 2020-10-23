using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Domain
{
    public class JournalItem
    {
        public Guid JournalItemId { get; set; }
        public Guid JournalId { get; set; }
        public Guid LedgerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime ReferenceDate { get; set; }

        public virtual Journal Journal { get; set; }
        public virtual Ledger Ledger { get; set; }
    }
}