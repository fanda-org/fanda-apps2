using Fanda.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class ProductVariety : OrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}
