using Fanda.Core;
using Fanda.Core.Base;
using System;

namespace Fanda.Accounting.Repository.Dto
{
    public class LedgerDto : BaseDto
    {
        public LedgerType LedgerType { get; set; }
        public Guid LedgerGroupId { get; set; }
        public bool IsSystem { get; set; }

        public virtual BankDto Bank { get; set; }
        public virtual PartyDto Party { get; set; }
        public virtual LedgerBalanceDto LedgerBalance { get; set; }
    }

    public class LedgerListDto : BaseListDto
    {
        public LedgerType LedgerType { get; set; }
        public string LedgerGroupName { get; set; }
    }
}