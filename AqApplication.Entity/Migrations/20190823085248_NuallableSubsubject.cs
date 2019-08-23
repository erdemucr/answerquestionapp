using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class NuallableSubsubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain");

            migrationBuilder.AlterColumn<int>(
                name: "SubSubjectId",
                table: "QuestionMain",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain",
                column: "SubSubjectId",
                principalTable: "SubSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain");

            migrationBuilder.AlterColumn<int>(
                name: "SubSubjectId",
                table: "QuestionMain",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionMain_SubSubjects_SubSubjectId",
                table: "QuestionMain",
                column: "SubSubjectId",
                principalTable: "SubSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
