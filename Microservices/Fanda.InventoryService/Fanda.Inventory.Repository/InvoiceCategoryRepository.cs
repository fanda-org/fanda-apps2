using AutoMapper;

using Fanda.Core.Base;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Domain.Context;
using Fanda.Inventory.Repository.Dto;

using System;
using System.Linq.Expressions;

namespace Fanda.Inventory.Repository
{
    public interface IInvoiceCategoryRepository :
        ISubRepository<InvoiceCategory, InvoiceCategoryDto, InvoiceCategoryListDto>
    {
    }

    public class InvoiceCategoryRepository :
        SubRepository<InvoiceCategory, InvoiceCategoryDto, InvoiceCategoryListDto>,
        IInvoiceCategoryRepository
    {
        public InvoiceCategoryRepository(InvtContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
        }

        protected override void SetSuperId(Guid superId, InvoiceCategory entity)
        {
            entity.OrgId = superId;
        }

        protected override Guid GetSuperId(InvoiceCategory entity)
        {
            return entity.OrgId;
        }

        protected override Expression<Func<InvoiceCategory, bool>> GetSuperIdPredicate(Guid superId)
        {
            return ic => ic.OrgId == superId;
        }
    }
}
