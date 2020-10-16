using AutoMapper;

using Fanda.Core.Base;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Domain.Context;
using Fanda.Inventory.Repository.Dto;

using System;
using System.Linq.Expressions;

namespace Fanda.Accounting.Repository
{
    public interface IProductSegmentRepository :
        ISubRepository<ProductSegment, ProductSegmentDto, ProductSegmentListDto>
    {
    }

    public class ProductSegmentRepository :
        SubRepository<ProductSegment, ProductSegmentDto, ProductSegmentListDto>,
        IProductSegmentRepository
    {
        public ProductSegmentRepository(InvtContext context, IMapper mapper)
            : base(context, mapper, "OrgId == @0")
        {
        }

        protected override void SetSuperId(Guid superId, ProductSegment entity)
        {
            entity.OrgId = superId;
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
