using Microsoft.EntityFrameworkCore.Migrations;

namespace FandaAuth.Domain.Migrations.MySQL
{
    public partial class MySQL03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceTypeString",
                table: "AppResources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceTypeString",
                table: "AppResources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
