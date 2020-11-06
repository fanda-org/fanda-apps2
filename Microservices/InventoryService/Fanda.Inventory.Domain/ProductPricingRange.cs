using System;
using Fanda.Core;

namespace Fanda.Inventory.Domain
{
    public class ProductPricingRange
    {
        public Guid RangeId { get; set; }
        public Guid PricingId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal AdjustPct { get; set; }
        public decimal AdjustAmt { get; set; }
        public RoundOffOption RoundOffOption { get; set; }

        public string RoundOffOptionString
        {
            get => RoundOffOption.ToString();
            set => RoundOffOption = (RoundOffOption)Enum.Parse(typeof(RoundOffOption), value, true);
        }

        public decimal FinalPrice { get; set; }

        public virtual ProductPricing ProductPricing { get; set; }
    }
}