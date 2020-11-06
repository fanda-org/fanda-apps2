using Fanda.Accounting.Domain.Base;
using System.Collections.Generic;

namespace Fanda.Accounting.Domain
{
    public class PartyType : OrgAcctEntity
    {
        public virtual ICollection<Party> Parties { get; set; }
    }
}