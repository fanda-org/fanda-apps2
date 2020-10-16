using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Deleteable",
                table: "AppResources");

            migrationBuilder.DropColumn(
                name: "Updateable",
                table: "AppResources");

            migrationBuilder.AddColumn<bool>(
                name: "Deletable",
                table: "AppResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Updatable",
                table: "AppResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    DateExpires = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    DateRevoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Deletable",
                table: "AppResources");

            migrationBuilder.DropColumn(
                name: "Updatable",
                table: "AppResources");

            migrationBuilder.AddColumn<bool>(
                name: "Deleteable",
                table: "AppResources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Updateable",
                table: "AppResources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedByIp = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", unicode: false, maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateExpires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateRevoked = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", unicode: false, maxLength: 100, nullable: true),
                    RevokedByIp = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", unicode: false, maxLength: 50, nullable: true),
                    Token = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", unicode: false, maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }
    }
}
