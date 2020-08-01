using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fanda.Domain.Context
{
    public class FandaContext : DbContext //IdentityDbContext<User, Role, Guid>
    {
        public FandaContext(DbContextOptions<FandaContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        //public DbSet<Role> Roles { get; set; }

        public DbSet<Unit> Units { get; set; }
        //public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductSegment> ProductSegments { get; set; }
        public DbSet<ProductVariety> ProductVarieties { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<LedgerGroup> LedgerGroups { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<PartyCategory> PartyCategories { get; set; }
        //public DbSet<Party> Parties { get; set; }
        //public DbSet<Bank> Banks { get; set; }
        public DbSet<AccountYear> AccountYears { get; set; }
        public DbSet<InvoiceCategory> InvoiceCategories { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        //public DbSet<InvoiceItem> InvoiceItems { get; set; }
        //public DbSet<Stock> Stock { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContactConfig());
            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new OrganizationConfig());
            modelBuilder.ApplyConfiguration(new OrgContactConfig());
            modelBuilder.ApplyConfiguration(new OrgAddressConfig());
            //modelBuilder.ApplyConfiguration(new OrgUserConfig());
            //modelBuilder.ApplyConfiguration(new RoleConfig());
            //modelBuilder.ApplyConfiguration(new OrgUserRoleConfig());

            modelBuilder.ApplyConfiguration(new UnitConfig());
            modelBuilder.ApplyConfiguration(new UnitConversionConfig());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductBrandConfig());
            modelBuilder.ApplyConfiguration(new ProductSegmentConfig());
            modelBuilder.ApplyConfiguration(new ProductVarietyConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new ProductIngredientConfig());

            modelBuilder.ApplyConfiguration(new LedgerGroupConfig());
            modelBuilder.ApplyConfiguration(new LedgerConfig());
            modelBuilder.ApplyConfiguration(new PartyCategoryConfig());
            modelBuilder.ApplyConfiguration(new PartyConfig());
            modelBuilder.ApplyConfiguration(new PartyContactConfig());
            modelBuilder.ApplyConfiguration(new PartyAddressConfig());
            modelBuilder.ApplyConfiguration(new BankConfig());
            modelBuilder.ApplyConfiguration(new AccountYearConfig());
            modelBuilder.ApplyConfiguration(new InvoiceCategoryConfig());

            modelBuilder.ApplyConfiguration(new ProductPricingConfig());
            modelBuilder.ApplyConfiguration(new ProductPrincingRangeConfig());

            modelBuilder.ApplyConfiguration(new LedgerBalanceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
            modelBuilder.ApplyConfiguration(new StockConfig());
            modelBuilder.ApplyConfiguration(new SerialNumberConfig());

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