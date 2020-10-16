using Fanda.Core.Base;
using System;

namespace Fanda.Accounting.Repository.Dto
{
    public class LedgerDto : BaseDto
    {
        //public Guid Id { get; set; }
        //public string LedgerCode { get; set; }
        //public string LedgerName { get; set; }
        //public string Description { get; set; }
        public Guid LedgerGroupId { get; set; }

        public string LedgerGroupName { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsSystem { get; set; }

        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }
        public virtual LedgerBalanceDto LedgerBalance { get; set; }
    }
}
