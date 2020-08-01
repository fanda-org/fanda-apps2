using System;
using System.Collections.Generic;

namespace Fanda.Core.Models
{
    public class ProductPricingDto
    {
        public ProductPricingDto()
        {
            PricingRanges = new HashSet<ProductPricingRangeDto>();
        }
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid PartyCategoryId { get; set; }
        public Guid InvoiceCategoryId { get; set; }

        public ICollection<ProductPricingRangeDto> PricingRanges { get; set; }
    }
}