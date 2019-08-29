using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class lecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChallengeTemplates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LectureId",
                table: "ChallengeTemplateItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_LectureId",
                table: "ChallengeTemplateItems",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeTemplateItems_Lectures_LectureId",
                table: "ChallengeTemplateItems",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeTemplateItems_Lectures_LectureId",
                table: "ChallengeTemplateItems");

            migrationBuilder.DropIndex(
                name: "IX_ChallengeTemplateItems_LectureId",
                table: "ChallengeTemplateItems");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "ChallengeTemplateItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChallengeTemplates",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
