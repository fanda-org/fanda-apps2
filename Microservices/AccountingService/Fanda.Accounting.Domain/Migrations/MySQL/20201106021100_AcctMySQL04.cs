using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartyAddresses");

            migrationBuilder.DropTable(
                name: "PartyContacts");

            migrationBuilder.DropColumn(
                name: "GSTIN",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "PAN",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "PartyType",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "RegdNum",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "TAN",
                table: "Parties");

            migrationBuilder.AddColumn<Guid>(
                name: "PartyOrgId",
                table: "Parties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PartyTypeId",
                table: "Parties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.CreateTable(
                name: "PartyTypes",
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
                    table.PrimaryKey("PK_PartyTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyTypes_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyOrgId",
                table: "Parties",
                column: "PartyOrgId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyTypeId",
                table: "Parties",
                column: "PartyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_OrgId",
                table: "PartyTypes",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_Code_OrgId",
                table: "PartyTypes",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyTypes_Name_OrgId",
                table: "PartyTypes",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Organizations_PartyOrgId",
                table: "Parties",
                column: "PartyOrgId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_PartyTypes_PartyTypeId",
                table: "Parties",
                column: "PartyTypeId",
                principalTable: "PartyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Organizations_PartyOrgId",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_PartyTypes_PartyTypeId",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "PartyTypes");

            migrationBuilder.DropIndex(
                name: "IX_Parties_PartyOrgId",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_PartyTypeId",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "PartyOrgId",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "PartyTypeId",
                table: "Parties");

            migrationBuilder.AddColumn<string>(
                name: "GSTIN",
                table: "Parties",
                type: "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PAN",
                table: "Parties",
                type: "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartyType",
                table: "Parties",
                type: "varchar(16) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegdNum",
                table: "Parties",
                type: "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TAN",
                table: "Parties",
                type: "varchar(25) CHARACTER SET utf8mb4",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Organizations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PartyAddresses",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AddressId = table.Column<Guid>(type: "char(36)", nullable: false)
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
                    PartyId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ContactId = table.Column<Guid>(type: "char(36)", nullable: false)
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
                name: "IX_PartyAddresses_AddressId",
                table: "PartyAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyContacts_ContactId",
                table: "PartyContacts",
                column: "ContactId");
        }
    }
}
