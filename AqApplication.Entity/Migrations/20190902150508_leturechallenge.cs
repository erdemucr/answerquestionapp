using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class leturechallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "ChallengeTemplates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplates_LectureId",
                table: "ChallengeTemplates",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeTemplates_Lectures_LectureId",
                table: "ChallengeTemplates",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeTemplates_Lectures_LectureId",
                table: "ChallengeTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeTemplates_LectureId",
                table: "ChallengeTemplates");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "ChallengeTemplates");
        }
    }
}
