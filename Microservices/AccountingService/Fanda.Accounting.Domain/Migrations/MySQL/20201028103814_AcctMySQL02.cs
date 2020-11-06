using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "OrgUsers",
                table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUsers", x => new { x.OrgId, x.UserId });
                    table.ForeignKey(
                        "FK_OrgUsers_Organizations_OrgId",
                        x => x.OrgId,
                        "Organizations",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "OrgUserRoles",
                table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUserRoles", x => new { x.OrgId, x.UserId, x.RoleId });
                    table.ForeignKey(
                        "FK_OrgUserRoles_OrgUsers_OrgId_UserId",
                        x => new { x.OrgId, x.UserId },
                        "OrgUsers",
                        new[] { "OrgId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "OrgUserRoles");

            migrationBuilder.DropTable(
                "OrgUsers");
        }
    }
}