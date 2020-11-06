using Fanda.Core.Base;
using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class Organization : BaseEntity
    {
        public Guid? TenantId { get; set; }
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        public virtual Party OrgParty { get; set; }
        public virtual ICollection<OrgContact> OrgContacts { get; set; }
        public virtual ICollection<OrgAddress> OrgAddresses { get; set; }
        public virtual ICollection<OrgUser> OrgUsers { get; set; }
        public virtual ICollection<PartyType> PartyTypes { get; set; }
        public virtual ICollection<PartyCategory> PartyCategories { get; set; }
        public virtual ICollection<Ledger> Ledgers { get; set; }
        public virtual ICollection<LedgerGroup> LedgerGroups { get; set; }
        public virtual ICollection<AccountYear> AccountYears { get; set; }
    }
}

//public virtual ICollection<Product> Products { get; set; }
//public virtual ICollection<ProductCategory> ProductCategories { get; set; }
//public virtual ICollection<ProductBrand> ProductBrands { get; set; }
//public virtual ICollection<ProductSegment> ProductSegments { get; set; }
//public virtual ICollection<ProductVariety> ProductVarieties { get; set; }
//public virtual ICollection<Unit> Units { get; set; }
//public virtual ICollection<InvoiceCategory> InvoiceCategories { get; set; }