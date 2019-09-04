using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class imagesize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeightImage",
                table: "QuestionMain",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WidthImage",
                table: "QuestionMain",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightImage",
                table: "QuestionMain");

            migrationBuilder.DropColumn(
                name: "WidthImage",
                table: "QuestionMain");
        }
    }
}
