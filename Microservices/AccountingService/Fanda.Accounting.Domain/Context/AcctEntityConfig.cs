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
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

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
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

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
            builder.Property(o => o.Id).ValueGeneratedOnAdd();

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

    public class OrgUserConfig : IEntityTypeConfiguration<OrgUser>
    {
        public void Configure(EntityTypeBuilder<OrgUser> builder)
        {
            // table
            builder.ToTable("OrgUsers");

            // key
            builder.HasKey(ou => new { ou.OrgId, ou.UserId });

            // foreign key
            builder.HasOne(ou => ou.Organization)
                .WithMany(b => b.OrgUsers)
                .HasForeignKey(ou => ou.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            // builder.HasOne(ou => ou.User)
            //     .WithMany(c => c.OrgUsers)
            //     .HasForeignKey(ou => ou.UserId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrgUserRoleConfig : IEntityTypeConfiguration<OrgUserRole>
    {
        public void Configure(EntityTypeBuilder<OrgUserRole> builder)
        {
            // table
            builder.ToTable("OrgUserRoles");

            // key
            builder.HasKey(o => new { o.OrgId, o.UserId, o.RoleId });

            // foreign key
            builder.HasOne(o => o.OrgUser)
                .WithMany(u => u.OrgUserRoles)
                .HasForeignKey(o => new { o.OrgId, o.UserId })
                .OnDelete(DeleteBehavior.Cascade);
            //builder.HasOne(o => o.Role)
            //    .WithMany(r => r.OrgUserRoles)
            //    .HasForeignKey(o => o.RoleId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Organization, Contact, Address, Role, User

    #region Ledger

    public class LedgerGroupConfig : IEntityTypeConfiguration<LedgerGroup>
    {
        public void Configure(EntityTypeBuilder<LedgerGroup> builder)
        {
            // table
            builder.ToTable("LedgerGroups");

            // key
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).ValueGeneratedOnAdd();

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
            builder.Property(l => l.Id).ValueGeneratedOnAdd();

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
            builder.Property(pc => pc.Id).ValueGeneratedOnAdd();

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
            builder.HasKey(y => y.Id);
            builder.Property(y => y.Id).ValueGeneratedOnAdd();

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

    #region Journal & Transaction

    public class JournalConfig : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            // table
            builder.ToTable("Journals");

            // primary key
            builder.HasKey(j => j.Id);
            builder.Property(j => j.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(i => i.Number)
                .HasMaxLength(16);
            builder.Property(i => i.ReferenceNumber)
                .HasMaxLength(16);
            builder.Ignore(i => i.JournalType);
            builder.Property(i => i.JournalTypeString)
                .HasColumnName("JournalType")
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(i => i.JournalSign)
                .HasMaxLength(1);

            // foreign key
            builder.HasOne(a => a.Ledger)
                .WithMany(b => b.Journals)
                .HasForeignKey(a => a.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AccountYear)
                .WithMany(b => b.Journals)
                .HasForeignKey(a => a.YearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class JournalItemConfig : IEntityTypeConfiguration<JournalItem>
    {
        public void Configure(EntityTypeBuilder<JournalItem> builder)
        {
            // table
            builder.ToTable("JournalItems");

            // key
            builder.HasKey(j => new { j.JournalItemId, j.JournalId });
            builder.Property(j => j.JournalItemId).ValueGeneratedOnAdd();

            // columns
            builder.Property(i => i.Description)
                .HasMaxLength(255);
            builder.Property(i => i.ReferenceNumber)
                .HasMaxLength(16);

            // foreign key
            builder.HasOne(ji => ji.Journal)
                .WithMany(j => j.JournalItems)
                .HasForeignKey(ji => ji.JournalId);
            builder.HasOne(a => a.Ledger)
                .WithMany(b => b.JournalItems)
                .HasForeignKey(a => a.LedgerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            // table
            builder.ToTable("Transactions");

            // key
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(i => i.Number)
                .HasMaxLength(16);
            builder.Property(i => i.ReferenceNumber)
                .HasMaxLength(16);
            builder.Property(i => i.Description)
                .HasMaxLength(255);

            // foreign keys
            builder.HasOne(t => t.DebitLedger)
                .WithMany(l => l.DebitTransactions)
                .HasForeignKey(t => t.DebitLedgerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CreditLedger)
                .WithMany(l => l.CreditTransactions)
                .HasForeignKey(t => t.CreditLedgerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AccountYear)
                .WithMany(b => b.Transactions)
                .HasForeignKey(a => a.YearId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Journal)
                .WithMany(j => j.Transactions)
                .HasForeignKey(t => t.JournalId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Journal & Transaction
}
