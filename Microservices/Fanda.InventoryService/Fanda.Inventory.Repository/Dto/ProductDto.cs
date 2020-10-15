using Fanda.Core;
using Fanda.Core.Base;

using System;
using System.Collections.Generic;

namespace Fanda.Inventory.Repository.Dto
{
    public class ProductDto : BaseDto
    {
        //public ProductDto()
        //{
        //    Ingredients = new HashSet<ProductIngredientDto>();
        //    ProductPricings = new HashSet<ProductPricingDto>();
        //}
        public ProductType ProductType { get; set; }

        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Guid SegmentId { get; set; }
        public Guid VarietyId { get; set; }
        public Guid UnitId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal CentralGstPct { get; set; }
        public decimal StateGstPct { get; set; }
        public decimal InterGstPct { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsCompoundProduct { get; set; }

        public ICollection<ProductIngredientDto> Ingredients { get; set; }
        public ICollection<ProductPricingDto> ProductPricings { get; set; }
    }
}
