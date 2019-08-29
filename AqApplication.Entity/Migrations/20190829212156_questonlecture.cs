using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class questonlecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "QuestionMain",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMain_LectureId",
                table: "QuestionMain",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionMain_Lectures_LectureId",
                table: "QuestionMain",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionMain_Lectures_LectureId",
                table: "QuestionMain");

            migrationBuilder.DropIndex(
                name: "IX_QuestionMain_LectureId",
                table: "QuestionMain");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "QuestionMain");
        }
    }
}
