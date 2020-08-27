using Fanda.Core;
using Fanda.Domain.Base;
using System;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class Ledger : OrgEntity
    {
        //public Guid Id { get; set; }
        //public string LedgerCode { get; set; }
        //public string LedgerName { get; set; }
        //public string Description { get; set; }

        public LedgerType LedgerType { get; set; }

        public string LedgerTypeString
        {
            get { return LedgerType.ToString(); }
            set { LedgerType = (LedgerType)Enum.Parse(typeof(LedgerType), value, true); }
        }

        public Guid LedgerGroupId { get; set; }

        //public Guid? ParentId { get; set; }
        public bool IsSystem { get; set; }

        //public Guid OrgId { get; set; }
        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        public virtual LedgerGroup LedgerGroup { get; set; }
        //public virtual Ledger Parent { get; set; }
        //public virtual ICollection<Ledger> Children { get; set; }
        //public virtual Organization Organization { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<LedgerBalance> LedgerBalances { get; set; }
    }
}
