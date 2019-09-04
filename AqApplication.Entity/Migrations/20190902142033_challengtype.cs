using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class challengtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ChallengeTemplates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExamLectures_LectureId",
                table: "ExamLectures",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamLectures_Lectures_LectureId",
                table: "ExamLectures",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamLectures_Lectures_LectureId",
                table: "ExamLectures");

            migrationBuilder.DropIndex(
                name: "IX_ExamLectures_LectureId",
                table: "ExamLectures");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ChallengeTemplates");
        }
    }
}
