using Microsoft.EntityFrameworkCore.Migrations;

namespace FandaAuth.Domain.Migrations.MySQL
{
    public partial class MySQL05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ResetPassword",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPassword",
                table: "Users");
        }
    }
}
