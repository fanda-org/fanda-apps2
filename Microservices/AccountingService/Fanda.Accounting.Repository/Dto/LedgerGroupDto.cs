using System;
using Fanda.Core;
using Fanda.Core.Base;

namespace Fanda.Accounting.Repository.Dto
{
    public class LedgerGroupDto : BaseDto
    {
        public LedgerGroupType GroupType { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsSystem { get; set; }
    }

    public class LedgerGroupListDto : BaseListDto
    {
        public LedgerGroupType GroupType { get; set; }
    }
}