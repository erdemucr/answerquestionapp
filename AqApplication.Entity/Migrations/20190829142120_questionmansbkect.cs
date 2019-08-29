using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class questionmansbkect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "QuestionMain",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMain_SubjectId",
                table: "QuestionMain",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionMain_Subjects_SubjectId",
                table: "QuestionMain",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionMain_Subjects_SubjectId",
                table: "QuestionMain");

            migrationBuilder.DropIndex(
                name: "IX_QuestionMain_SubjectId",
                table: "QuestionMain");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "QuestionMain");
        }
    }
}
