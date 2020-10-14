using Fanda.Accounting.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class ProductSegment : OrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}
