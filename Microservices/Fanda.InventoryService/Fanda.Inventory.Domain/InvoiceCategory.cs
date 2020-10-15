using Fanda.Inventory.Domain.Base;

using System.Collections.Generic;

namespace Fanda.Inventory.Domain
{
    public class InvoiceCategory : OrgInvtEntity
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
        public virtual ICollection<ProductPricing> ProductPricings { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
