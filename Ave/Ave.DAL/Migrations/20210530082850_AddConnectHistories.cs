using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ave.DAL.Migrations
{
    public partial class AddConnectHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    OrganId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectHistories_ArtificialOrgans_OrganId",
                        column: x => x.OrganId,
                        principalTable: "ArtificialOrgans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConnectHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectHistories_OrganId",
                table: "ConnectHistories",
                column: "OrganId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectHistories_UserId",
                table: "ConnectHistories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectHistories");
        }
    }
}
