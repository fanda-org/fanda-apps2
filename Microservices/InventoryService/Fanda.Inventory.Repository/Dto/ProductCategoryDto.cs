using System;
using Fanda.Core.Base;

namespace Fanda.Inventory.Repository.Dto
{
    public class ProductCategoryDto : BaseDto
    {
        public Guid? ParentId { get; set; }
    }

    public class ProductCategoryListDto : BaseListDto
    {
        public string ParentName { get; set; }
    }
}