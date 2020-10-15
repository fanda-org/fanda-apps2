using Fanda.Core;
using Fanda.Inventory.Domain.Base;

using System;
using System.Collections.Generic;

namespace Fanda.Inventory.Domain
{
    public class Product : OrgInvtEntity
    {
        //public Guid Id { get; set; }
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        public ProductType ProductType { get; set; }

        public string ProductTypeString
        {
            get { return ProductType.ToString(); }
            set { ProductType = (ProductType)Enum.Parse(typeof(ProductType), value, true); }
        }

        public Guid CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? SegmentId { get; set; }
        public Guid? VarietyId { get; set; }
        public Guid UnitId { get; set; }

        #region Tax / GST

        public string TaxCode { get; set; } //HSN code for Goods; SAC code for Service
        public ProductTaxPreference TaxPreference { get; set; }

        public string TaxPreferenceString
        {
            get { return TaxPreference.ToString(); }
            set { TaxPreference = (ProductTaxPreference)Enum.Parse(typeof(ProductTaxPreference), value, true); }
        }

        public decimal CentralGstPct { get; set; }
        public decimal StateGstPct { get; set; }
        public decimal InterGstPct { get; set; }

        #endregion Tax / GST

        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        //public Guid OrgId { get; set; }
        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual ProductBrand Brand { get; set; }
        public virtual ProductSegment Segment { get; set; }
        public virtual ProductVariety Variety { get; set; }
        public virtual Unit Unit { get; set; }

        //public virtual Organization Organization { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

        public virtual ICollection<ProductPricing> ProductPricings { get; set; }
        public virtual ICollection<ProductIngredient> ParentIngredients { get; set; }   // ProductIngredient.ProductId
        public virtual ICollection<ProductIngredient> ChildIngredients { get; set; }    // ProductIngredient.IngredientProductId
    }
}
