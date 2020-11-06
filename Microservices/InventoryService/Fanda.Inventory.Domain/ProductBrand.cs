﻿using System.Collections.Generic;
using Fanda.Inventory.Domain.Base;

namespace Fanda.Inventory.Domain
{
    public class ProductBrand : OrgInvtEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}