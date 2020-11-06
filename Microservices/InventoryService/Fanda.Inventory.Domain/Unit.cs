using System.Collections.Generic;
using Fanda.Inventory.Domain.Base;

namespace Fanda.Inventory.Domain
{
    public class Unit : OrgInvtEntity
    {
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
        public virtual ICollection<UnitConversion> FromUnitConversions { get; set; }
        public virtual ICollection<UnitConversion> ToUnitConversions { get; set; }
    }
}