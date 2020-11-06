using System;

namespace Fanda.Inventory.Domain
{
    public class ProductIngredient
    {
        //public Guid Id { get; set; }
        public Guid ParentProductId { get; set; }

        public Guid ChildProductId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }

        public virtual Product ParentProduct { get; set; }
        public virtual Product ChildProduct { get; set; }
        public virtual Unit Unit { get; set; }
    }
}