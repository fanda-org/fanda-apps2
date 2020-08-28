using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Base;
using Fanda.Service.Dto;

namespace Fanda.Service
{
    public interface IInvoiceCategoryRepository :
        IOrgRepository<InvoiceCategoryDto, InvoiceCategoryListDto>
    {
    }

    public class InvoiceCategoryRepository :
        OrgRepositoryBase<InvoiceCategory, InvoiceCategoryDto, InvoiceCategoryListDto>,
        IInvoiceCategoryRepository
    {
        public InvoiceCategoryRepository(FandaContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
