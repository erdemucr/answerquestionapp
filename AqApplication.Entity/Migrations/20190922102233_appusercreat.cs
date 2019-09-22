using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class appusercreat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Creator",
                table: "AspNetUsers",
                column: "Creator");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_Creator",
                table: "AspNetUsers",
                column: "Creator",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_Creator",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Creator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "AspNetUsers");
        }
    }
}
