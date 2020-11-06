using System.Collections.Generic;
using Fanda.Inventory.Domain.Base;

namespace Fanda.Inventory.Domain
{
    public class ProductVariety : OrgInvtEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}