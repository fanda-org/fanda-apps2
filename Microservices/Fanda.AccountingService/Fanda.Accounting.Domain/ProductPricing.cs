using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class ProductPricing
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? PartyCategoryId { get; set; }
        public Guid? InvoiceCategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual PartyCategory PartyCategory { get; set; }
        public virtual InvoiceCategory InvoiceCategory { get; set; }
        public virtual ICollection<ProductPricingRange> PricingRanges { get; set; }
    }
}
