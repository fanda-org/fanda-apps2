using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FandaAuth.Domain
{
    #region Application configuration

    public class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            // table
            builder.ToTable("Applications");

            // key
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(a => a.Edition)
                .IsUnicode(false)
                .HasMaxLength(25);
            builder.Property(a => a.Version)
                .IsUnicode(false)
                .HasMaxLength(16);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    public class AppResourceConfig : IEntityTypeConfiguration<AppResource>
    {
        public void Configure(EntityTypeBuilder<AppResource> builder)
        {
            // table
            builder.ToTable("AppResources");

            // key
            builder.HasKey(ar => ar.Id);
            builder.Property(ar => ar.Id).ValueGeneratedOnAdd();

            builder.Ignore(ar => ar.ResourceTypeString);

            // index
            builder.HasIndex(ar => new { ar.ApplicationId, ar.Code })
                .IsUnique();
            builder.HasIndex(ar => new { ar.ApplicationId, ar.Name })
                .IsUnique();

            // foreign key
            builder.HasOne(ar => ar.Application)
                .WithMany(a => a.AppResources)
                .HasForeignKey(ar => ar.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Application configuration

    #region Role privileges

    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            // table
            builder.ToTable("RolePrivileges");

            // key
            builder.HasKey(p => new { p.RoleId, p.AppResourceId });

            // index

            // foreign keys
            builder.HasOne(p => p.Role)
                .WithMany(r => r.Privileges)
                .HasForeignKey(p => new { p.RoleId })
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.AppResource)
                .WithMany(ra => ra.Privileges)
                .HasForeignKey(p => new { p.AppResourceId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    #endregion Role privileges

    #region Tenants configuration

    public class TenantConfig : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // table
            builder.ToTable("Tenants");

            // key
            builder.HasKey(t => t.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .HasMaxLength(255);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // table
            builder.ToTable("Users");

            // key
            builder.HasKey(u => new { u.Id });
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .IsUnicode(false)
                .IsFixedLength()
                .HasMaxLength(255);
            builder.Property(u => u.PasswordSalt)
                .IsRequired()
                .IsUnicode(false)
                .IsFixedLength()
                .HasMaxLength(255);
            builder.Property(u => u.FirstName)
                .HasMaxLength(50);
            builder.Property(u => u.LastName)
                .HasMaxLength(50);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(u => u.UserName)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Foreign keys - Owns
            //builder.OwnsMany(u => u.RefreshTokens);
            builder.HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            // table
            builder.ToTable("UserTokens");

            // key
            builder.HasKey(u => new { u.Id });
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Token)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);
            builder.Property(u => u.CreatedByIp)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(u => u.RevokedByIp)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(u => u.ReplacedByToken)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(100);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // index

            // Foreign keys - Owns
            builder.HasOne(u => u.User)
                .WithMany(t => t.RefreshTokens)
                .HasForeignKey(u => new { u.UserId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // table
            builder.ToTable("Roles");

            // key
            builder.HasKey(r => new { r.Id });
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(r => r.Description)
                .HasMaxLength(255);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(r => new { r.Code, r.TenantId })
                .IsUnique();
            builder.HasIndex(r => new { r.Name, r.TenantId })
                .IsUnique();

            // foreign key
            builder.HasOne(r => r.Tenant)
                .WithMany(o => o.Roles)
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    #endregion Tenants configuration
}
