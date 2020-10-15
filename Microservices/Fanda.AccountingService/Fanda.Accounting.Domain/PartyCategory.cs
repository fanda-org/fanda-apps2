using Fanda.Accounting.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class PartyCategory : OrgAcctEntity
    {
        //public Guid Id { get; set; }
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public Guid OrgId { get; set; }
        //public bool Active { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        //public virtual Organization Organization { get; set; }
        public virtual ICollection<Party> Parties { get; set; }

        //public virtual ICollection<ProductPricing> ProductPricings { get; set; }
    }
}
