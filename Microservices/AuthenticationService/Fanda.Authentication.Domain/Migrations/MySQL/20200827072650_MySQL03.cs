using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ResourceTypeString",
                "AppResources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "ResourceTypeString",
                "AppResources",
                "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}