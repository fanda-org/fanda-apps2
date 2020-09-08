using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Dto;
using System;
using System.Linq.Expressions;

namespace Fanda.Service
{
    public interface IProductSegmentRepository :
        ISubRepository<ProductSegment, ProductSegmentDto, ProductSegmentListDto>
    {
    }

    public class ProductSegmentRepository :
        SubRepository<ProductSegment, ProductSegmentDto, ProductSegmentListDto>,
        IProductSegmentRepository
    {
        public ProductSegmentRepository(FandaContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
        }

        protected override Guid GetSuperId(ProductSegment entity)
        {
            return entity.OrgId;
        }

        protected override Expression<Func<ProductSegment, bool>> GetSuperIdPredicate(Guid superId)
        {
            return ps => ps.OrgId == superId;
        }
    }
}
