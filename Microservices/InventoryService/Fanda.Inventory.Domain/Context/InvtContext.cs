using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fanda.Inventory.Domain.Context
{
    public class InvtContext : DbContext //IdentityDbContext<User, Role, Guid>
    {
        public InvtContext(DbContextOptions<InvtContext> options)
            : base(options)
        {
        }

        public DbSet<Unit> Units { get; set; }

        //public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductSegment> ProductSegments { get; set; }
        public DbSet<ProductVariety> ProductVarieties { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<InvoiceCategory> InvoiceCategories { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        //public DbSet<InvoiceItem> InvoiceItems { get; set; }
        //public DbSet<Stock> Stock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UnitConfig());
            modelBuilder.ApplyConfiguration(new UnitConversionConfig());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductBrandConfig());
            modelBuilder.ApplyConfiguration(new ProductSegmentConfig());
            modelBuilder.ApplyConfiguration(new ProductVarietyConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new ProductIngredientConfig());

            modelBuilder.ApplyConfiguration(new InvoiceCategoryConfig());

            modelBuilder.ApplyConfiguration(new ProductPricingConfig());
            modelBuilder.ApplyConfiguration(new ProductPrincingRangeConfig());

            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
            modelBuilder.ApplyConfiguration(new StockConfig());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetColumnType("decimal(18, 4)");
                //.Relational().ColumnType = "decimal(18, 4)";
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}