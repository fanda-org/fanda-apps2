using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Authentication.Domain.Migrations.MySQL
{
    public partial class MySQL01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Applications",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Edition = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    Version = table.Column<string>(unicode: false, maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Tenants",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    OrgCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "AppResources",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    ResourceType = table.Column<int>(nullable: false),
                    ResourceTypeString = table.Column<string>(nullable: true),
                    Creatable = table.Column<bool>(nullable: false),
                    Updateable = table.Column<bool>(nullable: false),
                    Deleteable = table.Column<bool>(nullable: false),
                    Readable = table.Column<bool>(nullable: false),
                    Printable = table.Column<bool>(nullable: false),
                    Importable = table.Column<bool>(nullable: false),
                    Exportable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppResources", x => x.Id);
                    table.ForeignKey(
                        "FK_AppResources_Applications_ApplicationId",
                        x => x.ApplicationId,
                        "Applications",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Roles",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        "FK_Roles_Tenants_TenantId",
                        x => x.TenantId,
                        "Tenants",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    PasswordHash =
                        table.Column<string>(unicode: false, fixedLength: true, maxLength: 255, nullable: false),
                    PasswordSalt =
                        table.Column<string>(unicode: false, fixedLength: true, maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    DateLastLogin = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        "FK_Users_Tenants_TenantId",
                        x => x.TenantId,
                        "Tenants",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "RolePrivileges",
                table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    AppResourceId = table.Column<Guid>(nullable: false),
                    Create = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Print = table.Column<bool>(nullable: false),
                    Import = table.Column<bool>(nullable: false),
                    Export = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => new { x.RoleId, x.AppResourceId });
                    table.ForeignKey(
                        "FK_RolePrivileges_AppResources_AppResourceId",
                        x => x.AppResourceId,
                        "AppResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RolePrivileges_Roles_RoleId",
                        x => x.RoleId,
                        "Roles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RefreshTokens",
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
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        "FK_RefreshTokens_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Applications_Code",
                "Applications",
                "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Applications_Name",
                "Applications",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AppResources_ApplicationId_Code",
                "AppResources",
                new[] { "ApplicationId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AppResources_ApplicationId_Name",
                "AppResources",
                new[] { "ApplicationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_RefreshTokens_UserId",
                "RefreshTokens",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_RolePrivileges_AppResourceId",
                "RolePrivileges",
                "AppResourceId");

            migrationBuilder.CreateIndex(
                "IX_Roles_TenantId",
                "Roles",
                "TenantId");

            migrationBuilder.CreateIndex(
                "IX_Roles_Code_TenantId",
                "Roles",
                new[] { "Code", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Roles_Name_TenantId",
                "Roles",
                new[] { "Name", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Tenants_Code",
                "Tenants",
                "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Tenants_Name",
                "Tenants",
                "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Users_Email",
                "Users",
                "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Users_TenantId",
                "Users",
                "TenantId");

            migrationBuilder.CreateIndex(
                "IX_Users_UserName",
                "Users",
                "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "RefreshTokens");

            migrationBuilder.DropTable(
                "RolePrivileges");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "AppResources");

            migrationBuilder.DropTable(
                "Roles");

            migrationBuilder.DropTable(
                "Applications");

            migrationBuilder.DropTable(
                "Tenants");
        }
    }
}