﻿using System;

namespace Fanda.Inventory.Repository.Dto
{
    public class ProductIngredientDto
    {
        public Guid ParentProductId { get; set; }
        public Guid ChildProductId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }
    }
}