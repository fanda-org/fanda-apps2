using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Dto;
using System;
using System.Linq.Expressions;

namespace Fanda.Service
{
    public interface IInvoiceCategoryRepository :
        ISubRepository<InvoiceCategory, InvoiceCategoryDto, InvoiceCategoryListDto>
    {
    }

    public class InvoiceCategoryRepository :
        SubRepository<InvoiceCategory, InvoiceCategoryDto, InvoiceCategoryListDto>,
        IInvoiceCategoryRepository
    {
        public InvoiceCategoryRepository(FandaContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
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
