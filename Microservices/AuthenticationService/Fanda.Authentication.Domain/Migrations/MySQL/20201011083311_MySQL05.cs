using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "ResetPassword",
                "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ResetPassword",
                "Users");
        }
    }
}