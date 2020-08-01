using System.Collections.Generic;

namespace Fanda.Domain
{
    public class ProductSegment : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}