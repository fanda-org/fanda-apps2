using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Base;
using Fanda.Service.Dto;

namespace Fanda.Service
{
    public interface IProductVarietyRepository :
        IOrgRepository<ProductVarietyDto, ProductVarietyListDto>
    {
    }

    public class ProductVarietyRepository :
        OrgRepositoryBase<ProductVariety, ProductVarietyDto, ProductVarietyListDto>,
        IProductVarietyRepository
    {
        public ProductVarietyRepository(FandaContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
