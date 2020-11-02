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
    public interface IJournalRepository :
        ISubRepository<Journal, JournalDto, JournalListDto>
    {
    }

    public class JournalRepository :
        SubRepository<Journal, JournalDto, JournalListDto>,
        IJournalRepository
    {
        public JournalRepository(AcctContext context, IMapper mapper)
            : base(context, mapper, "YearId ==@0")
        {
        }

        protected override Guid GetSuperId(Journal entity)
        {
            return entity.YearId;
        }

        protected override Expression<Func<Journal, bool>> GetSuperIdPredicate(Guid superId)
        {
            return j => j.YearId == superId;
        }

        protected override void SetSuperId(Guid superId, Journal entity)
        {
            entity.YearId = superId;
        }
    }
}
