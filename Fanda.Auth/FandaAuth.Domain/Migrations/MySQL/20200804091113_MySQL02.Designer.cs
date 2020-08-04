﻿// <auto-generated />
using System;
using FandaAuth.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FandaAuth.Domain.Migrations.MySQL
{
    [DbContext(typeof(AuthContext))]
    [Migration("20200804091113_MySQL02")]
    partial class MySQL02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FandaAuth.Domain.AppResource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("Creatable")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deletable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Exportable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Importable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("Printable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Readable")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ResourceType")
                        .HasColumnType("int");

                    b.Property<string>("ResourceTypeString")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Updatable")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId", "Code")
                        .IsUnique();

                    b.HasIndex("ApplicationId", "Name")
                        .IsUnique();

                    b.ToTable("AppResources");
                });

            modelBuilder.Entity("FandaAuth.Domain.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Edition")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Version")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("FandaAuth.Domain.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<Guid>("TenantId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("Code", "TenantId")
                        .IsUnique();

                    b.HasIndex("Name", "TenantId")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("FandaAuth.Domain.RolePrivilege", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AppResourceId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Create")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Delete")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Export")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Import")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Print")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Read")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Update")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("RoleId", "AppResourceId");

                    b.HasIndex("AppResourceId");

                    b.ToTable("RolePrivileges");
                });

            modelBuilder.Entity("FandaAuth.Domain.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<int>("OrgCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("FandaAuth.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateLastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("char(255) CHARACTER SET utf8mb4")
                        .IsFixedLength(true)
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("char(255) CHARACTER SET utf8mb4")
                        .IsFixedLength(true)
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<Guid>("TenantId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("TenantId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FandaAuth.Domain.UserToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateExpires")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateRevoked")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("FandaAuth.Domain.AppResource", b =>
                {
                    b.HasOne("FandaAuth.Domain.Application", "Application")
                        .WithMany("AppResources")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FandaAuth.Domain.Role", b =>
                {
                    b.HasOne("FandaAuth.Domain.Tenant", "Tenant")
                        .WithMany("Roles")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("FandaAuth.Domain.RolePrivilege", b =>
                {
                    b.HasOne("FandaAuth.Domain.AppResource", "AppResource")
                        .WithMany("Privileges")
                        .HasForeignKey("AppResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FandaAuth.Domain.Role", "Role")
                        .WithMany("Privileges")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FandaAuth.Domain.User", b =>
                {
                    b.HasOne("FandaAuth.Domain.Tenant", "Tenant")
                        .WithMany("Users")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("FandaAuth.Domain.UserToken", b =>
                {
                    b.HasOne("FandaAuth.Domain.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
