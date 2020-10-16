using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class MySQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Ledgers_ParentId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_ParentId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Ledgers");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "Products",
                unicode: false,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16) CHARACTER SET utf8mb4",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyType",
                table: "Parties",
                unicode: false,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16) CHARACTER SET utf8mb4",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedgerType",
                table: "Ledgers",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StockInvoiceType",
                table: "Invoices",
                unicode: false,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16) CHARACTER SET utf8mb4",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceType",
                table: "Invoices",
                unicode: false,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16) CHARACTER SET utf8mb4",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Banks",
                unicode: false,
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16) CHARACTER SET utf8mb4",
                oldMaxLength: 16,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LedgerType",
                table: "Ledgers");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "Products",
                type: "varchar(16) CHARACTER SET utf8mb4",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "PartyType",
                table: "Parties",
                type: "varchar(16) CHARACTER SET utf8mb4",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 16);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Ledgers",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockInvoiceType",
                table: "Invoices",
                type: "varchar(16) CHARACTER SET utf8mb4",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceType",
                table: "Invoices",
                type: "varchar(16) CHARACTER SET utf8mb4",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Banks",
                type: "varchar(16) CHARACTER SET utf8mb4",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_ParentId",
                table: "Ledgers",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Ledgers_ParentId",
                table: "Ledgers",
                column: "ParentId",
                principalTable: "Ledgers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
