using Fanda.Shared;
using System;

namespace Fanda.Core.Models
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