using Fanda.Accounting.Domain.Base;
using Fanda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository.Dto
{
    public class JournalDto : YearAcctEntity
    {
        public JournalType JournalType { get; set; }
        public string JournalSign { get; set; }
        public Guid LedgerId { get; set; }

        public virtual ICollection<JournalItemDto> JournalItems { get; set; }
        public virtual ICollection<TransactionDto> Transactions { get; set; }
    }

    public class JournalListDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public JournalType JournalType { get; set; }
        public string LedgerName { get; set; }
    }
}
