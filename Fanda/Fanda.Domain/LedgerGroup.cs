﻿using Fanda.Shared;
using System;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class LedgerGroup : BaseOrgEntity
    {
        //public Guid Id { get; set; }
        //public string GroupCode { get; set; }
        //public string GroupName { get; set; }
        //public string Description { get; set; }
        public LedgerGroupType GroupType { get; set; }
        public string GroupTypeString
        {
            get { return GroupType.ToString(); }
            set { GroupType = (LedgerGroupType)Enum.Parse(typeof(LedgerGroupType), value, true); }
        }
        public Guid? ParentId { get; set; }
        public bool IsSystem { get; set; }
        //public Guid OrgId { get; set; }
        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        public virtual LedgerGroup Parent { get; set; }
        public virtual ICollection<LedgerGroup> Children { get; set; }
        //public virtual Organization Organization { get; set; }
        public virtual ICollection<Ledger> Ledgers { get; set; }
    }
}
