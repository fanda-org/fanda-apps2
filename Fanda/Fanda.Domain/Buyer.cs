using System;
using System.Collections.Generic;

namespace Fanda.Domain
{
    public class Buyer
    {
        public Guid Id { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? AddressId { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
