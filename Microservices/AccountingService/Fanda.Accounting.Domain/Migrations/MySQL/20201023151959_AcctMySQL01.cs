using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Addresses",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Attention = table.Column<string>(maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    AddressLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: true),
                    State = table.Column<string>(maxLength: 25, nullable: true),
                    Country = table.Column<string>(maxLength: 25, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 25, nullable: true),
                    Fax = table.Column<string>(maxLength: 25, nullable: true),
                    AddressType = table.Column<string>(unicode: false, maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Contacts",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Salutation = table.Column<string>(maxLength: 5, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Designation = table.Column<string>(maxLength: 25, nullable: true),
                    Department = table.Column<string>(maxLength: 25, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    WorkPhone = table.Column<string>(maxLength: 25, nullable: true),
                    Mobile = table.Column<string>(maxLength: 25, nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Organizations",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "AccountYears",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    YearBegin = table.Column<DateTime>(nullable: false),
                    YearEnd = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountYears", x => x.Id);
                    table.ForeignKey(
                        "FK_AccountYears_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "LedgerGroups",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    GroupType = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerGroups", x => x.Id);
                    table.ForeignKey(
                        "FK_LedgerGroups_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_LedgerGroups_LedgerGroups_ParentId",
                        x => x.ParentId,
                        "LedgerGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "OrgAddresses",
                table => new
                {
                    OrgId = table.Column<Guid>(nullable: false), AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAddresses", x => new {x.OrgId, x.AddressId});
                    table.ForeignKey(
                        "FK_OrgAddresses_Addresses_AddressId",
                        x => x.AddressId,
                        "Addresses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_OrgAddresses_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "OrgContacts",
                table => new
                {
                    OrgId = table.Column<Guid>(nullable: false), ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgContacts", x => new {x.OrgId, x.ContactId});
                    table.ForeignKey(
                        "FK_OrgContacts_Contacts_ContactId",
                        x => x.ContactId,
                        "Contacts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_OrgContacts_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PartyCategories",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    OrgId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyCategories", x => x.Id);
                    table.ForeignKey(
                        "FK_PartyCategories_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SerialNumbers",
                table => new
                {
                    YearId = table.Column<Guid>(nullable: false),
                    Module = table.Column<string>(maxLength: 16, nullable: false),
                    Prefix = table.Column<string>(maxLength: 5, nullable: true),
                    SerialFormat = table.Column<string>(maxLength: 25, nullable: true),
                    Suffix = table.Column<string>(maxLength: 5, nullable: true),
                    LastValue = table.Column<string>(maxLength: 25, nullable: true),
                    LastNumber = table.Column<int>(nullable: false),
                    LastDate = table.Column<DateTime>(nullable: false),
                    Reset = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumbers", x => new {x.YearId, x.Module});
                    table.ForeignKey(
                        "FK_SerialNumbers_AccountYears_YearId",
                        x => x.YearId,
                        "AccountYears",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Ledgers",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    LedgerType = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    LedgerGroupId = table.Column<Guid>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        "FK_Ledgers_LedgerGroups_LedgerGroupId",
                        x => x.LedgerGroupId,
                        "LedgerGroups",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Ledgers_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Banks",
                table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    IfscCode = table.Column<string>(maxLength: 16, nullable: true),
                    MicrCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchName = table.Column<string>(maxLength: 50, nullable: true),
                    ContactId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.LedgerId);
                    table.ForeignKey(
                        "FK_Banks_Addresses_AddressId",
                        x => x.AddressId,
                        "Addresses",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Banks_Contacts_ContactId",
                        x => x.ContactId,
                        "Contacts",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Banks_Ledgers_LedgerId",
                        x => x.LedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Journals",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 16, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ReferenceNumber = table.Column<string>(maxLength: 16, nullable: true),
                    ReferenceDate = table.Column<DateTime>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    JournalType = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    JournalSign = table.Column<string>(maxLength: 1, nullable: true),
                    LedgerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                    table.ForeignKey(
                        "FK_Journals_Ledgers_LedgerId",
                        x => x.LedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Journals_AccountYears_YearId",
                        x => x.YearId,
                        "AccountYears",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "LedgerBalances",
                table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    OpeningBalance = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    BalanceSign = table.Column<string>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerBalances", x => new {x.LedgerId, x.YearId});
                    table.ForeignKey(
                        "FK_LedgerBalances_Ledgers_LedgerId",
                        x => x.LedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_LedgerBalances_AccountYears_YearId",
                        x => x.YearId,
                        "AccountYears",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Parties",
                table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true),
                    PartyType = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    PaymentTerm = table.Column<string>(maxLength: 16, nullable: true),
                    CreditLimit = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.LedgerId);
                    table.ForeignKey(
                        "FK_Parties_PartyCategories_CategoryId",
                        x => x.CategoryId,
                        "PartyCategories",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Parties_Ledgers_LedgerId",
                        x => x.LedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Transactions",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(maxLength: 16, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ReferenceNumber = table.Column<string>(maxLength: 16, nullable: true),
                    ReferenceDate = table.Column<DateTime>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    DebitLedgerId = table.Column<Guid>(nullable: false),
                    CreditLedgerId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    Amount = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        "FK_Transactions_Ledgers_CreditLedgerId",
                        x => x.CreditLedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Transactions_Ledgers_DebitLedgerId",
                        x => x.DebitLedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Transactions_AccountYears_YearId",
                        x => x.YearId,
                        "AccountYears",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "JournalItems",
                table => new
                {
                    JournalItemId = table.Column<Guid>(nullable: false),
                    JournalId = table.Column<Guid>(nullable: false),
                    LedgerId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    Amount = table.Column<decimal>("decimal(18, 4)", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceNumber = table.Column<string>(maxLength: 16, nullable: true),
                    ReferenceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalItems", x => new {x.JournalItemId, x.JournalId});
                    table.ForeignKey(
                        "FK_JournalItems_Journals_JournalId",
                        x => x.JournalId,
                        "Journals",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_JournalItems_Ledgers_LedgerId",
                        x => x.LedgerId,
                        "Ledgers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "PartyAddresses",
                table => new
                {
                    PartyId = table.Column<Guid>(nullable: false), AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyAddresses", x => new {x.PartyId, x.AddressId});
                    table.ForeignKey(
                        "FK_PartyAddresses_Addresses_AddressId",
                        x => x.AddressId,
                        "Addresses",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_PartyAddresses_Parties_PartyId",
                        x => x.PartyId,
                        "Parties",
                        "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "PartyContacts",
                table => new
                {
                    PartyId = table.Column<Guid>(nullable: false), ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyContacts", x => new {x.PartyId, x.ContactId});
                    table.ForeignKey(
                        "FK_PartyContacts_Contacts_ContactId",
                        x => x.ContactId,
                        "Contacts",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_PartyContacts_Parties_PartyId",
                        x => x.PartyId,
                        "Parties",
                        "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_AccountYears_OrgId",
                "AccountYears",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_AccountYears_Code_OrgId",
                "AccountYears",
                new[] {"Code", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Banks_AccountNumber",
                "Banks",
                "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Banks_AddressId",
                "Banks",
                "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Banks_ContactId",
                "Banks",
                "ContactId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_JournalItems_JournalId",
                "JournalItems",
                "JournalId");

            migrationBuilder.CreateIndex(
                "IX_JournalItems_LedgerId",
                "JournalItems",
                "LedgerId");

            migrationBuilder.CreateIndex(
                "IX_Journals_LedgerId",
                "Journals",
                "LedgerId");

            migrationBuilder.CreateIndex(
                "IX_Journals_YearId",
                "Journals",
                "YearId");

            migrationBuilder.CreateIndex(
                "IX_LedgerBalances_YearId",
                "LedgerBalances",
                "YearId");

            migrationBuilder.CreateIndex(
                "IX_LedgerGroups_OrgId",
                "LedgerGroups",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_LedgerGroups_ParentId",
                "LedgerGroups",
                "ParentId");

            migrationBuilder.CreateIndex(
                "IX_LedgerGroups_Code_OrgId",
                "LedgerGroups",
                new[] {"Code", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_LedgerGroups_Name_OrgId",
                "LedgerGroups",
                new[] {"Name", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Ledgers_LedgerGroupId",
                "Ledgers",
                "LedgerGroupId");

            migrationBuilder.CreateIndex(
                "IX_Ledgers_OrgId",
                "Ledgers",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_Ledgers_Code_OrgId",
                "Ledgers",
                new[] {"Code", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Ledgers_Name_OrgId",
                "Ledgers",
                new[] {"Name", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_OrgAddresses_AddressId",
                "OrgAddresses",
                "AddressId");

            migrationBuilder.CreateIndex(
                "IX_Organizations_Code",
                "Organizations",
                "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Organizations_Name",
                "Organizations",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_OrgContacts_ContactId",
                "OrgContacts",
                "ContactId");

            migrationBuilder.CreateIndex(
                "IX_Parties_CategoryId",
                "Parties",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_PartyAddresses_AddressId",
                "PartyAddresses",
                "AddressId");

            migrationBuilder.CreateIndex(
                "IX_PartyCategories_OrgId",
                "PartyCategories",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_PartyCategories_Code_OrgId",
                "PartyCategories",
                new[] {"Code", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_PartyCategories_Name_OrgId",
                "PartyCategories",
                new[] {"Name", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_PartyContacts_ContactId",
                "PartyContacts",
                "ContactId");

            migrationBuilder.CreateIndex(
                "IX_Transactions_CreditLedgerId",
                "Transactions",
                "CreditLedgerId");

            migrationBuilder.CreateIndex(
                "IX_Transactions_DebitLedgerId",
                "Transactions",
                "DebitLedgerId");

            migrationBuilder.CreateIndex(
                "IX_Transactions_YearId",
                "Transactions",
                "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Banks");

            migrationBuilder.DropTable(
                "JournalItems");

            migrationBuilder.DropTable(
                "LedgerBalances");

            migrationBuilder.DropTable(
                "OrgAddresses");

            migrationBuilder.DropTable(
                "OrgContacts");

            migrationBuilder.DropTable(
                "PartyAddresses");

            migrationBuilder.DropTable(
                "PartyContacts");

            migrationBuilder.DropTable(
                "SerialNumbers");

            migrationBuilder.DropTable(
                "Transactions");

            migrationBuilder.DropTable(
                "Journals");

            migrationBuilder.DropTable(
                "Addresses");

            migrationBuilder.DropTable(
                "Contacts");

            migrationBuilder.DropTable(
                "Parties");

            migrationBuilder.DropTable(
                "AccountYears");

            migrationBuilder.DropTable(
                "PartyCategories");

            migrationBuilder.DropTable(
                "Ledgers");

            migrationBuilder.DropTable(
                "LedgerGroups");

            migrationBuilder.DropTable(
                "Organizations");
        }
    }
}