﻿// <auto-generated />
using System;
using Fanda.Accounting.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    [DbContext(typeof(AcctContext))]
    [Migration("20201028103814_AcctMySQL02")]
    partial class AcctMySQL02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Fanda.Accounting.Domain.AccountYear", b =>
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

                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("YearBegin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("YearEnd")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("OrgId");

                    b.HasIndex("Code", "OrgId")
                        .IsUnique();

                    b.ToTable("AccountYears");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("AddressLine2")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("AddressTypeString")
                        .IsRequired()
                        .HasColumnName("AddressType")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25)
                        .IsUnicode(false);

                    b.Property<string>("Attention")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Country")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Fax")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("PostalCode")
                        .HasColumnType("varchar(10) CHARACTER SET utf8mb4")
                        .HasMaxLength(10);

                    b.Property<string>("State")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Bank", b =>
                {
                    b.Property<Guid>("LedgerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("AccountTypeString")
                        .IsRequired()
                        .HasColumnName("AccountType")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("char(36)");

                    b.Property<string>("BranchCode")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<string>("BranchName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<Guid?>("ContactId")
                        .HasColumnType("char(36)");

                    b.Property<string>("IfscCode")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MicrCode")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.HasKey("LedgerId");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ContactId")
                        .IsUnique();

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Department")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Designation")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Salutation")
                        .HasColumnType("varchar(5) CHARACTER SET utf8mb4")
                        .HasMaxLength(5);

                    b.Property<string>("WorkPhone")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Journal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("JournalSign")
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4")
                        .HasMaxLength(1);

                    b.Property<string>("JournalTypeString")
                        .IsRequired()
                        .HasColumnName("JournalType")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<Guid>("LedgerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Number")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<DateTime>("ReferenceDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<Guid>("YearId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("LedgerId");

                    b.HasIndex("YearId");

                    b.ToTable("Journals");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.JournalItem", b =>
                {
                    b.Property<Guid>("JournalItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("JournalId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<Guid>("LedgerId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime>("ReferenceDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.HasKey("JournalItemId", "JournalId");

                    b.HasIndex("JournalId");

                    b.HasIndex("LedgerId");

                    b.ToTable("JournalItems");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Ledger", b =>
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

                    b.Property<bool>("IsSystem")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("LedgerGroupId")
                        .HasColumnType("char(36)");

                    b.Property<string>("LedgerTypeString")
                        .IsRequired()
                        .HasColumnName("LedgerType")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("LedgerGroupId");

                    b.HasIndex("OrgId");

                    b.HasIndex("Code", "OrgId")
                        .IsUnique();

                    b.HasIndex("Name", "OrgId")
                        .IsUnique();

                    b.ToTable("Ledgers");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.LedgerBalance", b =>
                {
                    b.Property<Guid>("LedgerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("YearId")
                        .HasColumnType("char(36)");

                    b.Property<string>("BalanceSign")
                        .IsRequired()
                        .HasColumnType("varchar(1) CHARACTER SET utf8mb4")
                        .HasMaxLength(1);

                    b.Property<decimal>("OpeningBalance")
                        .HasColumnType("decimal(18, 4)");

                    b.HasKey("LedgerId", "YearId");

                    b.HasIndex("YearId");

                    b.ToTable("LedgerBalances");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.LedgerGroup", b =>
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

                    b.Property<string>("GroupTypeString")
                        .IsRequired()
                        .HasColumnName("GroupType")
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<bool>("IsSystem")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("OrgId");

                    b.HasIndex("ParentId");

                    b.HasIndex("Code", "OrgId")
                        .IsUnique();

                    b.HasIndex("Name", "OrgId")
                        .IsUnique();

                    b.ToTable("LedgerGroups");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgAddress", b =>
                {
                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrgId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("OrgAddresses");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgContact", b =>
                {
                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrgId", "ContactId");

                    b.HasIndex("ContactId");

                    b.ToTable("OrgContacts");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgUser", b =>
                {
                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrgId", "UserId");

                    b.ToTable("OrgUsers");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgUserRole", b =>
                {
                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

                    b.HasKey("OrgId", "UserId", "RoleId");

                    b.ToTable("OrgUserRoles");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Organization", b =>
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

                    b.Property<string>("GSTIN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<string>("PAN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("RegdNum")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("TAN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<Guid>("TenantId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Party", b =>
                {
                    b.Property<Guid>("LedgerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("CreditLimit")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<string>("GSTIN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("PAN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("PartyTypeString")
                        .IsRequired()
                        .HasColumnName("PartyType")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16)
                        .IsUnicode(false);

                    b.Property<string>("PaymentTermString")
                        .HasColumnName("PaymentTerm")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<string>("RegdNum")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("TAN")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.HasKey("LedgerId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyAddress", b =>
                {
                    b.Property<Guid>("PartyId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("char(36)");

                    b.HasKey("PartyId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("PartyAddresses");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyCategory", b =>
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

                    b.Property<Guid>("OrgId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("OrgId");

                    b.HasIndex("Code", "OrgId")
                        .IsUnique();

                    b.HasIndex("Name", "OrgId")
                        .IsUnique();

                    b.ToTable("PartyCategories");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyContact", b =>
                {
                    b.Property<Guid>("PartyId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("char(36)");

                    b.HasKey("PartyId", "ContactId");

                    b.HasIndex("ContactId");

                    b.ToTable("PartyContacts");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.SerialNumber", b =>
                {
                    b.Property<Guid>("YearId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ModuleString")
                        .HasColumnName("Module")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<DateTime>("LastDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LastNumber")
                        .HasColumnType("int");

                    b.Property<string>("LastValue")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Prefix")
                        .HasColumnType("varchar(5) CHARACTER SET utf8mb4")
                        .HasMaxLength(5);

                    b.Property<string>("ResetString")
                        .HasColumnName("Reset")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<string>("SerialFormat")
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25);

                    b.Property<string>("Suffix")
                        .HasColumnType("varchar(5) CHARACTER SET utf8mb4")
                        .HasMaxLength(5);

                    b.HasKey("YearId", "ModuleString");

                    b.ToTable("SerialNumbers");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<Guid>("CreditLedgerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DebitLedgerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4")
                        .HasMaxLength(255);

                    b.Property<string>("Number")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<DateTime>("ReferenceDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReferenceNumber")
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<Guid>("YearId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CreditLedgerId");

                    b.HasIndex("DebitLedgerId");

                    b.HasIndex("YearId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.AccountYear", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("AccountYears")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Bank", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Address", "Address")
                        .WithOne("Bank")
                        .HasForeignKey("Fanda.Accounting.Domain.Bank", "AddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fanda.Accounting.Domain.Contact", "Contact")
                        .WithOne("Bank")
                        .HasForeignKey("Fanda.Accounting.Domain.Bank", "ContactId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fanda.Accounting.Domain.Ledger", "Ledger")
                        .WithOne("Bank")
                        .HasForeignKey("Fanda.Accounting.Domain.Bank", "LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Journal", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Ledger", "Ledger")
                        .WithMany("Journals")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.AccountYear", "AccountYear")
                        .WithMany("Journals")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.JournalItem", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Journal", "Journal")
                        .WithMany("JournalItems")
                        .HasForeignKey("JournalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Ledger", "Ledger")
                        .WithMany("JournalItems")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Ledger", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.LedgerGroup", "LedgerGroup")
                        .WithMany("Ledgers")
                        .HasForeignKey("LedgerGroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("Ledgers")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.LedgerBalance", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Ledger", "Ledger")
                        .WithMany("LedgerBalances")
                        .HasForeignKey("LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.AccountYear", "AccountYear")
                        .WithMany("LedgerBalances")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.LedgerGroup", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("LedgerGroups")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.LedgerGroup", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgAddress", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Address", "Address")
                        .WithMany("OrgAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("OrgAddresses")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgContact", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Contact", "Contact")
                        .WithMany("OrgContacts")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("OrgContacts")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgUser", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("OrgUsers")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.OrgUserRole", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.OrgUser", "OrgUser")
                        .WithMany("OrgUserRoles")
                        .HasForeignKey("OrgId", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Party", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.PartyCategory", "Category")
                        .WithMany("Parties")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Ledger", "Ledger")
                        .WithOne("Party")
                        .HasForeignKey("Fanda.Accounting.Domain.Party", "LedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyAddress", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Address", "Address")
                        .WithMany("PartyAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Party", "Party")
                        .WithMany("PartyAddresses")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyCategory", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Organization", "Organization")
                        .WithMany("PartyCategories")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.PartyContact", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Contact", "Contact")
                        .WithMany("PartyContacts")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Party", "Party")
                        .WithMany("PartyContacts")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.SerialNumber", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.AccountYear", "AccountYear")
                        .WithMany("SerialNumbers")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fanda.Accounting.Domain.Transaction", b =>
                {
                    b.HasOne("Fanda.Accounting.Domain.Ledger", "CreditLedger")
                        .WithMany("CreditTransactions")
                        .HasForeignKey("CreditLedgerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.Ledger", "DebitLedger")
                        .WithMany("DebitTransactions")
                        .HasForeignKey("DebitLedgerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fanda.Accounting.Domain.AccountYear", "AccountYear")
                        .WithMany("Transactions")
                        .HasForeignKey("YearId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
