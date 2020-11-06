using Fanda.Core;
using Fanda.Core.Base;
using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Repository.Dto
{
    public class JournalDto : BaseYearDto
    {
        public JournalType JournalType { get; set; }
        public string JournalSign { get; set; }
        public Guid LedgerId { get; set; }

        public virtual ICollection<JournalItemDto> JournalItems { get; set; }
        public virtual ICollection<TransactionDto> Transactions { get; set; }
    }

    public class JournalListDto : BaseYearListDto
    {
        //public Guid Id { get; set; }
        //public string Number { get; set; }
        //public DateTime Date { get; set; }
        public JournalType JournalType { get; set; }

        //public string LedgerName { get; set; }
    }
}