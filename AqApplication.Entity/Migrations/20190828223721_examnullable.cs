using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class examnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeTemplateItems_Exams_ExamId",
                table: "ChallengeTemplateItems");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "ChallengeTemplateItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeTemplateItems_Exams_ExamId",
                table: "ChallengeTemplateItems",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeTemplateItems_Exams_ExamId",
                table: "ChallengeTemplateItems");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "ChallengeTemplateItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeTemplateItems_Exams_ExamId",
                table: "ChallengeTemplateItems",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
