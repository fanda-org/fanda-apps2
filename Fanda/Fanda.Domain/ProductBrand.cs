using System.Collections.Generic;

namespace Fanda.Domain
{
    public class ProductBrand : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}