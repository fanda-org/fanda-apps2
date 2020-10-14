using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fanda.Accounting.Domain.Context
{
    #region Global - Contact and Address

    public class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            // table
            builder.ToTable("Contacts");

            // key
            builder.HasKey(c => c.Id);

            // columns
            builder.Property(c => c.Salutation)
                .HasMaxLength(5);
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.LastName)
                .HasMaxLength(50);
            builder.Property(c => c.Designation)
                .HasMaxLength(25);
            builder.Property(c => c.Department)
                .HasMaxLength(25);
            builder.Property(c => c.Email)
                .HasMaxLength(100);
            builder.Property(c => c.WorkPhone)
                .HasMaxLength(25);
            builder.Property(c => c.Mobile)
                .HasMaxLength(25);
        }
    }

    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // table
            builder.ToTable("Addresses");

            // key
            builder.HasKey(a => a.Id);

            // columns
            builder.Property(a => a.Attention)
                .HasMaxLength(50);
            builder.Property(a => a.AddressLine1)
                .HasMaxLength(100);
            builder.Property(a => a.AddressLine2)
                .HasMaxLength(100);
            builder.Property(a => a.City)
                .HasMaxLength(25);
            builder.Property(a => a.State)
                .HasMaxLength(25);
            builder.Property(a => a.Country)
                .HasMaxLength(25);
            builder.Property(a => a.PostalCode)
                .HasMaxLength(10);
            builder.Property(a => a.Phone)
                .HasMaxLength(25);
            builder.Property(a => a.Fax)
                .HasMaxLength(25);
            builder.Ignore(a => a.AddressType);
            builder.Property(a => a.AddressTypeString)
                .HasColumnName("AddressType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(25);
        }
    }

    #endregion Global - Contact and Address

    #region Organization

    #region Organization, Contact, Address, Role, User

    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            // table
            builder.ToTable("Organizations");

            // key
            builder.HasKey(o => o.Id);

            // columns
            builder.Property(o => o.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(o => o.Description)
                .HasMaxLength(255);
            builder.Property(o => o.RegdNum)
                .HasMaxLength(25);
            builder.Property(o => o.PAN)
                .HasMaxLength(25);
            builder.Property(o => o.TAN)
                .HasMaxLength(25);
            builder.Property(o => o.GSTIN)
                .HasMaxLength(25);
            //builder.Property(o => o.DateCreated)
            //.HasDefaultValueSql("getdate()")
            //.ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified)
            //.HasDefaultValueSql("getdate()")
            //.ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(o => o.Code)
                .IsUnique();
            builder.HasIndex(o => o.Name)
                .IsUnique();
        }
    }

    public class OrgContactConfig : IEntityTypeConfiguration<OrgContact>
    {
        public void Configure(EntityTypeBuilder<OrgContact> builder)
        {
            // table
            builder.ToTable("OrgContacts");

            // key
            builder.HasKey(oc => new { oc.OrgId, oc.ContactId });

            // foreign key
            builder.HasOne(oc => oc.Organization)
                .WithMany(b => b.OrgContacts)
                .HasForeignKey(oc => oc.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oc => oc.Contact)
                .WithMany(c => c.OrgContacts)
                .HasForeignKey(oc => oc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrgAddressConfig : IEntityTypeConfiguration<OrgAddress>
    {
        public void Configure(EntityTypeBuilder<OrgAddress> builder)
        {
            // table
            builder.ToTable("OrgAddresses");

            // key
            builder.HasKey(oa => new { oa.OrgId, oa.AddressId });

            // foreign key
            builder.HasOne(oa => oa.Organization)
                .WithMany(b => b.OrgAddresses)
                .HasForeignKey(oa => oa.OrgId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oa => oa.Address)
                .WithMany(c => c.OrgAddresses)
                .HasForeignKey(oa => oa.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // public class OrgUserConfig : IEntityTypeConfiguration<OrgUser>
    // {
    //     public void Configure(EntityTypeBuilder<OrgUser> builder)
    //     {
    //         // table
    //         builder.ToTable("OrgUsers");

    //         // key
    //         builder.HasKey(ou => new { ou.OrgId, ou.UserId });

    //         // foreign key
    //         builder.HasOne(ou => ou.Organization)
    //             .WithMany(b => b.OrgUsers)
    //             .HasForeignKey(ou => ou.OrgId)
    //             .OnDelete(DeleteBehavior.Cascade);
    //         // builder.HasOne(ou => ou.User)
    //         //     .WithMany(c => c.OrgUsers)
    //         //     .HasForeignKey(ou => ou.UserId)
    //         //     .OnDelete(DeleteBehavior.Cascade);
    //     }
    // }
    // public class OrgUserRoleConfig : IEntityTypeConfiguration<OrgUserRole>
    // {
    //     public void Configure(EntityTypeBuilder<OrgUserRole> builder)
    //     {
    //         // table
    //         builder.ToTable("OrgUserRoles");

    //         // key
    //         builder.HasKey(o => new { o.OrgId, o.UserId, o.RoleId });

    //         // foreign key
    //         builder.HasOne(o => o.OrgUser)
    //             .WithMany(u => u.OrgUserRoles)
    //             .HasForeignKey(o => new { o.OrgId, o.UserId })
    //             .OnDelete(DeleteBehavior.Cascade);
    //         builder.HasOne(o => o.Role)
    //             .WithMany(r => r.OrgUserRoles)
    //             .HasForeignKey(o => o.RoleId)
    //             .OnDelete(DeleteBehavior.Cascade);
    //     }
    // }

    #endregion Organization, Contact, Address, Role, User

    #region Unit and Unit conversion

    public class UnitConfig : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            // table
            builder.ToTable("Units");

            // key
            builder.HasKey(u => u.Id);

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
            builder.HasOne(u => u.Organization)
                .WithMany(o => o.Units)
                .HasForeignKey(u => u.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductCategories)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductBrands)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductSegments)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductVarieties)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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

            // columns

            // index
            builder.HasIndex(pp => new { pp.ProductId, pp.PartyCategoryId, pp.InvoiceCategoryId })
                .IsUnique();

            // foreign key
            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPricings)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pp => pp.PartyCategory)
                .WithMany(pc => pc.ProductPricings)
                .HasForeignKey(pp => pp.PartyCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

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
            builder.HasOne(p => p.Organization)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Product

    #region Ledger

    public class LedgerGroupConfig : IEntityTypeConfiguration<LedgerGroup>
    {
        public void Configure(EntityTypeBuilder<LedgerGroup> builder)
        {
            // table
            builder.ToTable("LedgerGroups");

            // key
            builder.HasKey(u => u.Id);

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Ignore(g => g.GroupType);
            builder.Property(g => g.GroupTypeString)
                .HasColumnName("GroupType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(20);
            //builder.Property(u => u.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(u => u.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(g => new { g.Code, g.OrgId })
                .IsUnique();
            builder.HasIndex(g => new { g.Name, g.OrgId })
                .IsUnique();

            // foreign key
            builder.HasOne(g => g.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(g => g.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Organization)
                .WithMany(o => o.LedgerGroups)
                .HasForeignKey(u => u.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class LedgerConfig : IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            // table
            builder.ToTable("Ledgers");

            // key
            builder.HasKey(l => l.Id);

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Ignore(l => l.LedgerType);
            builder.Property(l => l.LedgerTypeString)
                .HasColumnName("LedgerType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(20);

            //builder.Property(pc => pc.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(pc => pc.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();

            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.Ledgers)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pc => pc.LedgerGroup)
                .WithMany(o => o.Ledgers)
                .HasForeignKey(pc => pc.LedgerGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(pc => pc.Parent)
            //    .WithMany(p => p.Children)
            //    .HasForeignKey(pc => pc.ParentId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Ledger

    #region Party

    public class PartyCategoryConfig : IEntityTypeConfiguration<PartyCategory>
    {
        public void Configure(EntityTypeBuilder<PartyCategory> builder)
        {
            // table
            builder.ToTable("PartyCategories");

            // key
            builder.HasKey(pc => pc.Id);

            // columns
            builder.Property(pc => pc.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pc => pc.Description)
                .HasMaxLength(255);
            //builder.Property(pc => pc.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(pc => pc.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(pc => new { pc.Code, pc.OrgId })
                .IsUnique();
            builder.HasIndex(pc => new { pc.Name, pc.OrgId })
                .IsUnique();

            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.PartyCategories)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PartyConfig : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            // table
            builder.ToTable("Parties");

            // key
            builder.HasKey(c => c.LedgerId);

            // columns
            builder.Ignore(pc => pc.PartyType);
            builder.Property(pc => pc.PartyTypeString)
                .HasColumnName("PartyType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(o => o.RegdNum)
                .HasMaxLength(25);
            builder.Property(o => o.PAN)
                .HasMaxLength(25);
            builder.Property(o => o.TAN)
                .HasMaxLength(25);
            builder.Property(o => o.GSTIN)
                .HasMaxLength(25);
            builder.Ignore(s => s.PaymentTerm);
            builder.Property(s => s.PaymentTermString)
                .HasColumnName("PaymentTerm")
                .HasMaxLength(16);

            // index

            // foreign key
            builder.HasOne(p => p.Ledger)
                .WithOne(o => o.Party)
                .HasForeignKey<Party>(p => p.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Category)
                .WithMany(pc => pc.Parties)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PartyContactConfig : IEntityTypeConfiguration<PartyContact>
    {
        public void Configure(EntityTypeBuilder<PartyContact> builder)
        {
            // table
            builder.ToTable("PartyContacts");

            // key
            builder.HasKey(oc => new { oc.PartyId, oc.ContactId });

            // foreign key
            builder.HasOne(oc => oc.Party)
                .WithMany(b => b.PartyContacts)
                .HasForeignKey(oc => oc.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oc => oc.Contact)
                .WithMany(c => c.PartyContacts)
                .HasForeignKey(oc => oc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PartyAddressConfig : IEntityTypeConfiguration<PartyAddress>
    {
        public void Configure(EntityTypeBuilder<PartyAddress> builder)
        {
            // table
            builder.ToTable("PartyAddresses");

            // key
            builder.HasKey(oa => new { oa.PartyId, oa.AddressId });

            // foreign key
            builder.HasOne(oa => oa.Party)
                .WithMany(b => b.PartyAddresses)
                .HasForeignKey(oa => oa.PartyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oa => oa.Address)
                .WithMany(c => c.PartyAddresses)
                .HasForeignKey(oa => oa.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Party

    public class BankConfig : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            // table
            builder.ToTable("Banks");

            // key
            builder.HasKey(b => b.LedgerId);

            // columns
            builder.Property(b => b.AccountNumber)
                .IsRequired()
                .HasMaxLength(25);
            builder.Ignore(b => b.AccountType);
            builder.Property(b => b.AccountTypeString)
                .HasColumnName("AccountType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(b => b.IfscCode)
                .HasMaxLength(16);
            builder.Property(b => b.MicrCode)
                .HasMaxLength(16);
            builder.Property(b => b.BranchCode)
                .HasMaxLength(16);
            builder.Property(b => b.BranchName)
                .HasMaxLength(50);

            // index
            builder.HasIndex(p => p.AccountNumber)
                .IsUnique();

            // foreign key
            builder.HasOne(b => b.Ledger)
                .WithOne(c => c.Bank)
                .HasForeignKey<Bank>(a => a.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Contact)
                .WithOne(c => c.Bank)
                .HasForeignKey<Bank>(a => a.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Address)
                .WithOne(a => a.Bank)
                .HasForeignKey<Bank>(a => a.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class AccountYearConfig : IEntityTypeConfiguration<AccountYear>
    {
        public void Configure(EntityTypeBuilder<AccountYear> builder)
        {
            // table
            builder.ToTable("AccountYears");

            // key
            builder.HasKey(u => u.Id);

            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Property(u => u.YearBegin)
                .IsRequired();
            builder.Property(u => u.YearEnd)
                .IsRequired();

            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();

            // foreign key
            builder.HasOne(u => u.Organization)
                .WithMany(o => o.AccountYears)
                .HasForeignKey(u => u.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class InvoiceCategoryConfig : IEntityTypeConfiguration<InvoiceCategory>
    {
        public void Configure(EntityTypeBuilder<InvoiceCategory> builder)
        {
            // table
            builder.ToTable("InvoiceCategories");

            // key
            builder.HasKey(ic => ic.Id);

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
            builder.HasOne(ic => ic.Organization)
                .WithMany(o => o.InvoiceCategories)
                .HasForeignKey(ic => ic.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
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

            // columns

            // index

            // foreign keys
            builder.HasOne(b => b.Contact)
                .WithOne(c => c.Buyer)
                .HasForeignKey<Buyer>(b => b.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Address)
                .WithOne(c => c.Buyer)
                .HasForeignKey<Buyer>(b => b.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Organization

    #region Accounting Year

    public class LedgerBalanceConfig : IEntityTypeConfiguration<LedgerBalance>
    {
        public void Configure(EntityTypeBuilder<LedgerBalance> builder)
        {
            // table
            builder.ToTable("LedgerBalances");

            // key
            builder.HasKey(u => new { u.LedgerId, u.YearId });

            // columns
            builder.Property(u => u.BalanceSign)
                .IsRequired()
                .HasMaxLength(1);

            // index

            // foreign key
            builder.HasOne(u => u.Ledger)
                .WithMany(o => o.LedgerBalances)
                .HasForeignKey(u => u.LedgerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.AccountYear)
                .WithMany(o => o.LedgerBalances)
                .HasForeignKey(u => u.YearId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // table
            builder.ToTable("Invoices");

            // key
            builder.HasKey(i => i.Id);

            // columns
            builder.Property(i => i.Number)
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
            builder.HasOne(i => i.AccountYear)
                .WithMany(y => y.Invoices)
                .HasForeignKey(i => i.YearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Party)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

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

    public class SerialNumberConfig : IEntityTypeConfiguration<SerialNumber>
    {
        public void Configure(EntityTypeBuilder<SerialNumber> builder)
        {
            // table
            builder.ToTable("SerialNumbers");

            // key
            builder.HasKey(sn => new { sn.YearId, sn.ModuleString });

            // columns
            builder.Ignore(sn => sn.Module);
            builder.Property(sn => sn.ModuleString)
                .HasColumnName("Module")
                .HasMaxLength(16);
            builder.Property(sn => sn.Prefix)
                .HasMaxLength(5);
            builder.Property(sn => sn.SerialFormat)
                .HasMaxLength(25);
            builder.Property(sn => sn.Suffix)
                .HasMaxLength(5);
            builder.Property(sn => sn.LastValue)
                .HasMaxLength(25);
            builder.Ignore(sn => sn.Reset);
            builder.Property(sn => sn.ResetString)
                .HasColumnName("Reset")
                .HasMaxLength(16);

            // index

            // foreign keys
            builder.HasOne(sn => sn.AccountYear)
                .WithMany(ay => ay.SerialNumbers)
                .HasForeignKey(sn => sn.YearId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Accounting Year
}
