using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Accounting.Domain.Migrations.MySQL
{
    public partial class AcctMySQL02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrgUsers",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUsers", x => new { x.OrgId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OrgUsers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgUserRoles",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUserRoles", x => new { x.OrgId, x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_OrgUserRoles_OrgUsers_OrgId_UserId",
                        columns: x => new { x.OrgId, x.UserId },
                        principalTable: "OrgUsers",
                        principalColumns: new[] { "OrgId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgUserRoles");

            migrationBuilder.DropTable(
                name: "OrgUsers");
        }
    }
}
