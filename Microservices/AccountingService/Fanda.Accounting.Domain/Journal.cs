using Fanda.Accounting.Domain.Base;
using Fanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Domain
{
    public class Journal : YearAcctEntity
    {
        public JournalType JournalType { get; set; }

        public string JournalTypeString
        {
            get { return JournalType.ToString(); }
            set { JournalType = (JournalType)Enum.Parse(typeof(JournalType), value, true); }
        }

        public string JournalSign { get; set; }
        public Guid LedgerId { get; set; }

        public virtual Ledger Ledger { get; set; }
        public virtual ICollection<JournalItem> JournalItems { get; set; }
    }
}