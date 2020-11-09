using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "Journals",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "Journals",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
