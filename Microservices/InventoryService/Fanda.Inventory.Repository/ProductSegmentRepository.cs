using System;
using System.Linq.Expressions;
using AutoMapper;
using Fanda.Core.Base;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Domain.Context;
using Fanda.Inventory.Repository.Dto;

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
            : base(context, mapper)
        {
        }

        public override Expression<Func<ProductSegment, bool>> GetSuperIdPredicate(Guid? superId)
        {
            return ps => ps.OrgId == superId;
        }

        protected override void SetSuperId(Guid superId, ProductSegment entity)
        {
            entity.OrgId = superId;
        }

        protected override Guid GetSuperId(ProductSegment entity)
        {
            return entity.OrgId;
        }
    }
}