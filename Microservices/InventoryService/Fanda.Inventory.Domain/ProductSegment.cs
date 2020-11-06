using Fanda.Inventory.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Inventory.Domain
{
    public class ProductSegment : OrgInvtEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}