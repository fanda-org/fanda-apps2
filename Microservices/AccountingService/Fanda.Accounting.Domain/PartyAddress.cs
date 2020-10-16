using System;

namespace Fanda.Accounting.Domain
{
    public class PartyAddress
    {
        public Guid PartyId { get; set; }
        public Guid AddressId { get; set; }

        public virtual Party Party { get; set; }
        public virtual Address Address { get; set; }
    }
}
