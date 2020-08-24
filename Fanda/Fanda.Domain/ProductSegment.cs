using Fanda.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class ProductSegment : OrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}
