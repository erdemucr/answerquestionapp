using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class HostCallLogingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostCallLogging",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Seo = table.Column<int>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Editor = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    RequestContentType = table.Column<string>(nullable: true),
                    RequestUri = table.Column<string>(nullable: true),
                    RequestMethod = table.Column<string>(nullable: true),
                    RequestTimestamp = table.Column<DateTime>(nullable: true),
                    ResponseContentType = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: false),
                    ResponseTimestamp = table.Column<DateTime>(nullable: true),
                    TimeInterval = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostCallLogging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HostCallLogging_AspNetUsers_Creator",
                        column: x => x.Creator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HostCallLogging_AspNetUsers_Editor",
                        column: x => x.Editor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HostCallLogging_Creator",
                table: "HostCallLogging",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_HostCallLogging_Editor",
                table: "HostCallLogging",
                column: "Editor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HostCallLogging");
        }
    }
}
