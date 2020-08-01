﻿using Fanda.Shared;
using System;

namespace Fanda.Core.Models
{
    public class ProductPricingRangeDto
    {
        public Guid PricingId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal AdjustPct { get; set; }
        public decimal AdjustAmt { get; set; }
        public RoundOffOption RoundOffOption { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
