﻿using AutoMapper;
using Fanda.Core.Base;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Domain.Context;
using Fanda.Inventory.Repository.Dto;
using System;
using System.Linq.Expressions;

namespace Fanda.Inventory.Repository
{
    public interface IProductVarietyRepository :
        ISubRepository<ProductVariety, ProductVarietyDto, ProductVarietyListDto>
    {
    }

    public class ProductVarietyRepository :
        SubRepository<ProductVariety, ProductVarietyDto, ProductVarietyListDto>,
        IProductVarietyRepository
    {
        public ProductVarietyRepository(InvtContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public override Expression<Func<ProductVariety, bool>> GetSuperIdPredicate(Guid? superId)
        {
            return pv => pv.OrgId == superId;
        }

        protected override void SetSuperId(Guid superId, ProductVariety entity)
        {
            entity.OrgId = superId;
        }

        protected override Guid GetSuperId(ProductVariety entity)
        {
            return entity.OrgId;
        }
    }
}