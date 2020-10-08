using Microsoft.EntityFrameworkCore.Migrations;

namespace FandaAuth.Domain.Migrations.MySQL
{
    public partial class MySQL04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_Token",
                table: "UserTokens",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTokens_Token",
                table: "UserTokens");
        }
    }
}
