using Fanda.Core;
using System;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Attention { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { set; get; }
        public string Fax { set; get; }
        public AddressType AddressType { get; set; }

        public string AddressTypeString
        {
            get { return AddressType.ToString(); }
            set { AddressType = (AddressType)Enum.Parse(typeof(AddressType), value, true); }
        }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<OrgAddress> OrgAddresses { get; set; }
        // public virtual ICollection<PartyAddress> PartyAddresses { get; set; }
    }
}