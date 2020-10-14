using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FandaAuth.Domain.Migrations.PgSQL
{
    public partial class PgSQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ResourceTypeString",
                table: "AppResources");

            migrationBuilder.RenameColumn(
                name: "Updateable",
                table: "AppResources",
                newName: "Updatable");

            migrationBuilder.RenameColumn(
                name: "Deleteable",
                table: "AppResources",
                newName: "Deletable");

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    DateExpires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    DateRevoked = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true)
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

            migrationBuilder.RenameColumn(
                name: "Updatable",
                table: "AppResources",
                newName: "Updateable");

            migrationBuilder.RenameColumn(
                name: "Deletable",
                table: "AppResources",
                newName: "Deleteable");

            migrationBuilder.AddColumn<string>(
                name: "ResourceTypeString",
                table: "AppResources",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByIp = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateExpires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateRevoked = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    RevokedByIp = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Token = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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
