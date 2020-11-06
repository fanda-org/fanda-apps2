using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fanda.Inventory.Domain.Context
{
    #region Unit and Unit conversion

    public class UnitConfig : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            // table
            builder.ToTable("Units");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(u => u.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(u => u.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(u => u.Organization)
            //    .WithMany(o => o.Units)
            //    .HasForeignKey(u => u.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class UnitConversionConfig : IEntityTypeConfiguration<UnitConversion>
    {
        public void Configure(EntityTypeBuilder<UnitConversion> builder)
        {
            // table
            builder.ToTable("UnitConversions");

            // key
            builder.HasKey(u => new { u.FromUnitId, u.ToUnitId });

            // columns

            // index

            // foreign key
            builder.HasOne(uc => uc.FromUnit)
                .WithMany(u => u.FromUnitConversions)
                .HasForeignKey(uc => uc.FromUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uc => uc.ToUnit)
                .WithMany(u => u.ToUnitConversions)
                .HasForeignKey(uc => uc.ToUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Unit and Unit conversion

    #region Product

    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            // table
            builder.ToTable("ProductCategories");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(pc => pc.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(pc => pc.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(pc => pc.Organization)
            //    .WithMany(o => o.ProductCategories)
            //    .HasForeignKey(pc => pc.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pc => pc.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(pc => pc.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductBrandConfig : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            // table
            builder.ToTable("ProductBrands");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(b => b.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(b => b.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(pc => pc.Organization)
            //    .WithMany(o => o.ProductBrands)
            //    .HasForeignKey(pc => pc.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductSegmentConfig : IEntityTypeConfiguration<ProductSegment>
    {
        public void Configure(EntityTypeBuilder<ProductSegment> builder)
        {
            // table
            builder.ToTable("ProductSegments");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(s => s.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(s => s.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(pc => pc.Organization)
            //    .WithMany(o => o.ProductSegments)
            //    .HasForeignKey(pc => pc.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductVarietyConfig : IEntityTypeConfiguration<ProductVariety>
    {
        public void Configure(EntityTypeBuilder<ProductVariety> builder)
        {
            // table
            builder.ToTable("ProductVarieties");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(v => v.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(v => v.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(pc => pc.Organization)
            //    .WithMany(o => o.ProductVarieties)
            //    .HasForeignKey(pc => pc.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductIngredientConfig : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            // table
            builder.ToTable("ProductIngredients");

            // key
            builder.HasKey(p => new { p.ParentProductId, p.ChildProductId, p.UnitId });

            // index
            //builder.HasIndex(p => new { p.ParentProductId, p.ChildProductId, p.UnitId })
            //    .IsUnique();

            // foreign key
            builder.HasOne(i => i.ParentProduct)
                .WithMany(p => p.ParentIngredients)
                .HasForeignKey(i => i.ParentProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.ChildProduct)
                .WithMany(p => p.ChildIngredients)
                .HasForeignKey(i => i.ChildProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Unit)
                .WithMany(u => u.ProductIngredients)
                .HasForeignKey(i => i.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductPricingConfig : IEntityTypeConfiguration<ProductPricing>
    {
        public void Configure(EntityTypeBuilder<ProductPricing> builder)
        {
            // table
            builder.ToTable("ProductPricings");

            // key
            builder.HasKey(pp => pp.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns

            // index
            builder.HasIndex(pp => new { pp.ProductId, pp.PartyCategoryId, pp.InvoiceCategoryId })
                .IsUnique();

            // foreign key
            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPricings)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(pp => pp.PartyCategory)
            //    .WithMany(pc => pc.ProductPricings)
            //    .HasForeignKey(pp => pp.PartyCategoryId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pp => pp.InvoiceCategory)
                .WithMany(ic => ic.ProductPricings)
                .HasForeignKey(pp => pp.InvoiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductPrincingRangeConfig : IEntityTypeConfiguration<ProductPricingRange>
    {
        public void Configure(EntityTypeBuilder<ProductPricingRange> builder)
        {
            // table
            builder.ToTable("ProductPricingRanges");

            // key
            builder.HasKey(r => new { r.RangeId, r.PricingId });

            // columns
            builder.Ignore(r => r.RoundOffOption);
            builder.Property(r => r.RoundOffOptionString)
                .HasColumnName("RoundOffOption")
                .HasMaxLength(16);

            // foreign key
            builder.HasOne(r => r.ProductPricing)
                .WithMany(pp => pp.PricingRanges)
                .HasForeignKey(r => r.PricingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // table
            builder.ToTable("Products");

            // key
            builder.HasKey(u => u.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Ignore(p => p.ProductType);
            builder.Property(p => p.ProductTypeString)
                .HasColumnName("ProductType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Ignore(p => p.TaxPreference);
            builder.Property(p => p.TaxPreferenceString)
                .HasColumnName("TaxPreference")
                .HasMaxLength(16);
            //builder.Property(p => p.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(p => p.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            builder.HasOne(p => p.Category)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Unit)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Segment)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.SegmentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Variety)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.VarietyId)
                .OnDelete(DeleteBehavior.Restrict);
            //builder.HasOne(p => p.Organization)
            //    .WithMany(o => o.Products)
            //    .HasForeignKey(p => p.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Product

    #region Organization

    public class InvoiceCategoryConfig : IEntityTypeConfiguration<InvoiceCategory>
    {
        public void Configure(EntityTypeBuilder<InvoiceCategory> builder)
        {
            // table
            builder.ToTable("InvoiceCategories");

            // key
            builder.HasKey(ic => ic.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(ic => ic.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(ic => ic.Name)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(ic => ic.Description)
                .HasMaxLength(255);
            //builder.Property(c => c.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(c => c.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(ic => new { ic.Code, ic.OrgId })
                .IsUnique();
            builder.HasIndex(ic => new { ic.Name, ic.OrgId })
                .IsUnique();

            // foreign key
            //builder.HasOne(ic => ic.Organization)
            //    .WithMany(o => o.InvoiceCategories)
            //    .HasForeignKey(ic => ic.OrgId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class BuyerConfig : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            // table
            builder.ToTable("Buyers");

            // key
            builder.HasKey(b => b.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns

            // index

            // foreign keys
            //builder.HasOne(b => b.Contact)
            //    .WithOne(c => c.Buyer)
            //    .HasForeignKey<Buyer>(b => b.ContactId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(b => b.Address)
            //    .WithOne(c => c.Buyer)
            //    .HasForeignKey<Buyer>(b => b.AddressId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Organization

    #region Accounting Year

    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // table
            builder.ToTable("Invoices");

            // key
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(i => i.Number)
                .HasMaxLength(16);
            builder.Property(i => i.ReferenceNumber)
                .HasMaxLength(16);
            builder.Ignore(i => i.InvoiceType);
            builder.Property(i => i.InvoiceTypeString)
                .HasColumnName("InvoiceType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(i => i.Notes)
                .HasMaxLength(255);
            builder.Property(i => i.PartyRefNum)
                .HasMaxLength(25);
            builder.Ignore(i => i.StockInvoiceType);
            builder.Property(i => i.StockInvoiceTypeString)
                .HasColumnName("StockInvoiceType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Ignore(i => i.GstTreatment);
            builder.Property(i => i.GstTreatmentString)
                .HasColumnName("GstTreatment")
                .HasMaxLength(16);
            builder.Ignore(i => i.TaxPreference);
            builder.Property(i => i.TaxPreferenceString)
                .HasColumnName("TaxPreference")
                .HasMaxLength(16);

            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(i => new { i.Number, i.YearId })
                .IsUnique();

            // foreign key
            //builder.HasOne(i => i.AccountYear)
            //    .WithMany(y => y.Invoices)
            //    .HasForeignKey(i => i.YearId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(i => i.Party)
            //    .WithMany(p => p.Invoices)
            //    .HasForeignKey(i => i.PartyId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Category)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Buyer)
                .WithMany(b => b.Invoices)
                .HasForeignKey(i => i.BuyerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            // table
            builder.ToTable("InvoiceItems");

            // key
            builder.HasKey(i => new { i.InvoiceItemId, i.InvoiceId });
            builder.Property(i => i.InvoiceItemId).ValueGeneratedOnAdd();

            // columns
            builder.Property(i => i.Description)
                .HasMaxLength(255);

            // foreign key
            builder.HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ii => ii.Stock)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ii => ii.Unit)
                .WithMany(u => u.InvoiceItems)
                .HasForeignKey(ii => ii.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            // table
            builder.ToTable("Stock");

            // key
            builder.HasKey(s => s.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(s => s.BatchNumber)
                .HasMaxLength(25);

            // index
            builder.HasIndex(s => new { s.BatchNumber, s.ProductId })
                .IsUnique();

            // foreign key
            builder.HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Unit)
                .WithMany(u => u.Stocks)
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Accounting Year
}