using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                "JournalId",
                "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_Transactions_JournalId",
                "Transactions",
                "JournalId");

            migrationBuilder.AddForeignKey(
                "FK_Transactions_Journals_JournalId",
                "Transactions",
                "JournalId",
                "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Transactions_Journals_JournalId",
                "Transactions");

            migrationBuilder.DropIndex(
                "IX_Transactions_JournalId",
                "Transactions");

            migrationBuilder.DropColumn(
                "JournalId",
                "Transactions");
        }
    }
}