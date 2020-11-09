using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "JournalItems",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReferenceDate",
                table: "JournalItems",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
