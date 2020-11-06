using System.Collections.Generic;
using Fanda.Accounting.Domain.Base;

namespace Fanda.Accounting.Domain
{
    public class PartyType : OrgAcctEntity
    {
        public virtual ICollection<Party> Parties { get; set; }
    }
}