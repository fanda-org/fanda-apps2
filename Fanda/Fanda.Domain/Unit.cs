using System.Collections.Generic;

namespace Fanda.Domain
{
    public class Unit : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
        public virtual ICollection<UnitConversion> FromUnitConversions { get; set; }
        public virtual ICollection<UnitConversion> ToUnitConversions { get; set; }
    }
}