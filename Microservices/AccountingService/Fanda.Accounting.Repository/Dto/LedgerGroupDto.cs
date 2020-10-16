using Fanda.Core;
using Fanda.Core.Base;
using System;

namespace Fanda.Accounting.Repository.Dto
{
    public class LedgerGroupDto : BaseDto
    {
        //public Guid Id { get; set; }
        //public string GroupCode { get; set; }
        //public string GroupName { get; set; }
        //public string Description { get; set; }
        public LedgerGroupType GroupType { get; set; }

        public Guid? ParentId { get; set; }
        public bool IsSystem { get; set; }
        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }
    }
}
