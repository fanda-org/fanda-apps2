using AutoMapper;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;
using System;
using System.Linq.Expressions;

namespace Fanda.Accounting.Repository
{
    public interface ILedgerRepository :
        ISubRepository<Ledger, LedgerDto, LedgerListDto>
    {
    }

    public class LedgerRepository :
        SubRepository<Ledger, LedgerDto, LedgerListDto>,
        ILedgerRepository
    {
        public LedgerRepository(AcctContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
        }

        protected override Guid GetSuperId(Ledger entity)
        {
            return entity.OrgId;
        }

        protected override Expression<Func<Ledger, bool>> GetSuperIdPredicate(Guid superId)
        {
            return l => l.OrgId == superId;
        }

        protected override void SetSuperId(Guid superId, Ledger entity)
        {
            entity.OrgId = superId;
        }
    }
}
