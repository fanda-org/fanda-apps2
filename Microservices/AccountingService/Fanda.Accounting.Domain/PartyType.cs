using Fanda.Accounting.Domain.Base;
using Fanda.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Domain
{
    public class PartyType : OrgAcctEntity
    {
        public virtual ICollection<Party> Parties { get; set; }
    }
}