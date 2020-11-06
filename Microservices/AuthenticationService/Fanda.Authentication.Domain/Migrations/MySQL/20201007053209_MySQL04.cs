using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                "IX_UserTokens_Token",
                "UserTokens",
                "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                "IX_UserTokens_Token",
                "UserTokens");
        }
    }
}