using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "RefreshTokens");

            migrationBuilder.DropColumn(
                "Deleteable",
                "AppResources");

            migrationBuilder.DropColumn(
                "Updateable",
                "AppResources");

            migrationBuilder.AddColumn<bool>(
                "Deletable",
                "AppResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "Updatable",
                "AppResources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                "UserTokens",
                table => new
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
                        "FK_UserTokens_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_UserTokens_UserId",
                "UserTokens",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "UserTokens");

            migrationBuilder.DropColumn(
                "Deletable",
                "AppResources");

            migrationBuilder.DropColumn(
                "Updatable",
                "AppResources");

            migrationBuilder.AddColumn<bool>(
                "Deleteable",
                "AppResources",
                "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                "Updateable",
                "AppResources",
                "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                "RefreshTokens",
                table => new
                {
                    Id = table.Column<Guid>("char(36)", nullable: false),
                    CreatedByIp =
                        table.Column<string>("varchar(50) CHARACTER SET utf8mb4", false, 50, nullable: false),
                    DateCreated = table.Column<DateTime>("datetime(6)", nullable: false),
                    DateExpires = table.Column<DateTime>("datetime(6)", nullable: false),
                    DateRevoked = table.Column<DateTime>("datetime(6)", nullable: true),
                    ReplacedByToken =
                        table.Column<string>("varchar(100) CHARACTER SET utf8mb4", false, 100, nullable: true),
                    RevokedByIp =
                        table.Column<string>("varchar(50) CHARACTER SET utf8mb4", false, 50, nullable: true),
                    Token = table.Column<string>("varchar(100) CHARACTER SET utf8mb4", false, 100, nullable: false),
                    UserId = table.Column<Guid>("char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        "FK_RefreshTokens_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_RefreshTokens_UserId",
                "RefreshTokens",
                "UserId");
        }
    }
}