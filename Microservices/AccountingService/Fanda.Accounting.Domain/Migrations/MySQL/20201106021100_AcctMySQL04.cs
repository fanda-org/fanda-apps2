using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PartyAddresses");

            migrationBuilder.DropTable(
                "PartyContacts");

            migrationBuilder.DropColumn(
                "GSTIN",
                "Parties");

            migrationBuilder.DropColumn(
                "PAN",
                "Parties");

            migrationBuilder.DropColumn(
                "PartyType",
                "Parties");

            migrationBuilder.DropColumn(
                "RegdNum",
                "Parties");

            migrationBuilder.DropColumn(
                "TAN",
                "Parties");

            migrationBuilder.AddColumn<Guid>(
                "PartyOrgId",
                "Parties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                "PartyTypeId",
                "Parties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                "TenantId",
                "Organizations",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.CreateTable(
                "PartyTypes",
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
                    table.PrimaryKey("PK_PartyTypes", x => x.Id);
                    table.ForeignKey(
                        "FK_PartyTypes_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Parties_PartyOrgId",
                "Parties",
                "PartyOrgId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Parties_PartyTypeId",
                "Parties",
                "PartyTypeId");

            migrationBuilder.CreateIndex(
                "IX_PartyTypes_OrgId",
                "PartyTypes",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_PartyTypes_Code_OrgId",
                "PartyTypes",
                new[] {"Code", "OrgId"},
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_PartyTypes_Name_OrgId",
                "PartyTypes",
                new[] {"Name", "OrgId"},
                unique: true);

            migrationBuilder.AddForeignKey(
                "FK_Parties_Organizations_PartyOrgId",
                "Parties",
                "PartyOrgId",
                "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Parties_PartyTypes_PartyTypeId",
                "Parties",
                "PartyTypeId",
                "PartyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Parties_Organizations_PartyOrgId",
                "Parties");

            migrationBuilder.DropForeignKey(
                "FK_Parties_PartyTypes_PartyTypeId",
                "Parties");

            migrationBuilder.DropTable(
                "PartyTypes");

            migrationBuilder.DropIndex(
                "IX_Parties_PartyOrgId",
                "Parties");

            migrationBuilder.DropIndex(
                "IX_Parties_PartyTypeId",
                "Parties");

            migrationBuilder.DropColumn(
                "PartyOrgId",
                "Parties");

            migrationBuilder.DropColumn(
                "PartyTypeId",
                "Parties");

            migrationBuilder.AddColumn<string>(
                "GSTIN",
                "Parties",
                "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "PAN",
                "Parties",
                "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "PartyType",
                "Parties",
                "varchar(16) CHARACTER SET utf8mb4",
                false,
                16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                "RegdNum",
                "Parties",
                "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "TAN",
                "Parties",
                "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                "TenantId",
                "Organizations",
                "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateTable(
                "PartyAddresses",
                table => new
                {
                    PartyId = table.Column<Guid>("char(36)", nullable: false),
                    AddressId = table.Column<Guid>("char(36)", nullable: false)
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
                    PartyId = table.Column<Guid>("char(36)", nullable: false),
                    ContactId = table.Column<Guid>("char(36)", nullable: false)
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
                "IX_PartyAddresses_AddressId",
                "PartyAddresses",
                "AddressId");

            migrationBuilder.CreateIndex(
                "IX_PartyContacts_ContactId",
                "PartyContacts",
                "ContactId");
        }
    }
}