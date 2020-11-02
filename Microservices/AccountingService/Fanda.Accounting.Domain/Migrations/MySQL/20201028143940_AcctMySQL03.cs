using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JournalId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_JournalId",
                table: "Transactions",
                column: "JournalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Journals_JournalId",
                table: "Transactions",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Journals_JournalId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_JournalId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Transactions");
        }
    }
}
