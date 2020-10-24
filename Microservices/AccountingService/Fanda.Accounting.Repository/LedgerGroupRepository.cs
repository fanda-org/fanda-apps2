using AutoMapper;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository
{
    public interface ILedgerGroupRepository :
        ISubRepository<LedgerGroup, LedgerGroupDto, LedgerGroupListDto>
    {
    }

    public class LedgerGroupRepository :
        SubRepository<LedgerGroup, LedgerGroupDto, LedgerGroupListDto>,
        ILedgerGroupRepository
    {
        public LedgerGroupRepository(AcctContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
        }

        protected override Guid GetSuperId(LedgerGroup entity)
        {
            return entity.OrgId;
        }

        protected override Expression<Func<LedgerGroup, bool>> GetSuperIdPredicate(Guid superId)
        {
            return g => g.OrgId == superId;
        }

        protected override void SetSuperId(Guid superId, LedgerGroup entity)
        {
            entity.OrgId = superId;
        }
    }
}
