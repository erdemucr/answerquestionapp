using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class SubSubjectAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubSubjectId",
                table: "QuestionMain",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMain_SubSubjectId",
                table: "QuestionMain",
                column: "SubSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain",
                column: "SubSubjectId",
                principalTable: "SubSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain");

            migrationBuilder.DropIndex(
                name: "IX_QuestionMain_SubSubjectId",
                table: "QuestionMain");

            migrationBuilder.DropColumn(
                name: "SubSubjectId",
                table: "QuestionMain");
        }
    }
}
