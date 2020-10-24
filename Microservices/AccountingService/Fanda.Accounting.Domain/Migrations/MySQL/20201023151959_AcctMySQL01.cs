using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
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
                name: "Contacts",
                columns: table => new
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
                name: "Organizations",
                columns: table => new
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
                name: "AccountYears",
                columns: table => new
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
                        name: "FK_AccountYears_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedgerGroups",
                columns: table => new
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
                        name: "FK_LedgerGroups_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgerGroups_LedgerGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LedgerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgAddresses",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAddresses", x => new { x.OrgId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_OrgAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgAddresses_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgContacts",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgContacts", x => new { x.OrgId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_OrgContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgContacts_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyCategories",
                columns: table => new
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
                        name: "FK_PartyCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SerialNumbers",
                columns: table => new
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
                    table.PrimaryKey("PK_SerialNumbers", x => new { x.YearId, x.Module });
                    table.ForeignKey(
                        name: "FK_SerialNumbers_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
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
                        name: "FK_Ledgers_LedgerGroups_LedgerGroupId",
                        column: x => x.LedgerGroupId,
                        principalTable: "LedgerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ledgers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
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
                        name: "FK_Banks_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banks_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banks_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
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
                        name: "FK_Journals_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Journals_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedgerBalances",
                columns: table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    BalanceSign = table.Column<string>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerBalances", x => new { x.LedgerId, x.YearId });
                    table.ForeignKey(
                        name: "FK_LedgerBalances_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LedgerBalances_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true),
                    PartyType = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    PaymentTerm = table.Column<string>(maxLength: 16, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_Parties_PartyCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PartyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parties_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
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
                    Quantity = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Ledgers_CreditLedgerId",
                        column: x => x.CreditLedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Ledgers_DebitLedgerId",
                        column: x => x.DebitLedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalItems",
                columns: table => new
                {
                    JournalItemId = table.Column<Guid>(nullable: false),
                    JournalId = table.Column<Guid>(nullable: false),
                    LedgerId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceNumber = table.Column<string>(maxLength: 16, nullable: true),
                    ReferenceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalItems", x => new { x.JournalItemId, x.JournalId });
                    table.ForeignKey(
                        name: "FK_JournalItems_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JournalItems_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyAddresses",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyAddresses", x => new { x.PartyId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_PartyAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyAddresses_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyContacts",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyContacts", x => new { x.PartyId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_PartyContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyContacts_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_OrgId",
                table: "AccountYears",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_Code_OrgId",
                table: "AccountYears",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AccountNumber",
                table: "Banks",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AddressId",
                table: "Banks",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_ContactId",
                table: "Banks",
                column: "ContactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalItems_JournalId",
                table: "JournalItems",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalItems_LedgerId",
                table: "JournalItems",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_LedgerId",
                table: "Journals",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Journals_YearId",
                table: "Journals",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerBalances_YearId",
                table: "LedgerBalances",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_OrgId",
                table: "LedgerGroups",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_ParentId",
                table: "LedgerGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_Code_OrgId",
                table: "LedgerGroups",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_Name_OrgId",
                table: "LedgerGroups",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerGroupId",
                table: "Ledgers",
                column: "LedgerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_OrgId",
                table: "Ledgers",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_Code_OrgId",
                table: "Ledgers",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_Name_OrgId",
                table: "Ledgers",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgAddresses_AddressId",
                table: "OrgAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Code",
                table: "Organizations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgContacts_ContactId",
                table: "OrgContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_CategoryId",
                table: "Parties",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddresses_AddressId",
                table: "PartyAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_OrgId",
                table: "PartyCategories",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_Code_OrgId",
                table: "PartyCategories",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_Name_OrgId",
                table: "PartyCategories",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyContacts_ContactId",
                table: "PartyContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditLedgerId",
                table: "Transactions",
                column: "CreditLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DebitLedgerId",
                table: "Transactions",
                column: "DebitLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_YearId",
                table: "Transactions",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "JournalItems");

            migrationBuilder.DropTable(
                name: "LedgerBalances");

            migrationBuilder.DropTable(
                name: "OrgAddresses");

            migrationBuilder.DropTable(
                name: "OrgContacts");

            migrationBuilder.DropTable(
                name: "PartyAddresses");

            migrationBuilder.DropTable(
                name: "PartyContacts");

            migrationBuilder.DropTable(
                name: "SerialNumbers");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Journals");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "AccountYears");

            migrationBuilder.DropTable(
                name: "PartyCategories");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "LedgerGroups");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
