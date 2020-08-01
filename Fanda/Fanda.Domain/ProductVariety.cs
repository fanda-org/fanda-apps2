using System.Collections.Generic;

namespace Fanda.Domain
{
    public class ProductVariety : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}