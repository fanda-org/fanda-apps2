using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Base;
using Fanda.Service.Dto;

namespace Fanda.Service
{
    public interface IProductSegmentRepository :
        IOrgRepository<ProductSegmentDto, ProductSegmentListDto>
    {
    }

    public class ProductSegmentRepository :
        OrgRepositoryBase<ProductSegment, ProductSegmentDto, ProductSegmentListDto>,
        IProductSegmentRepository
    {
        public ProductSegmentRepository(FandaContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
