using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class isdeletedaddded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubSubjects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionPdfs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionMain",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionExams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuestionClass",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Lectures",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HostCallLogging",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExamLectures",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Difficulty",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ConfigurationValues",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Classes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChallengeTypes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChallengeTemplates",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChallengeTemplateItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ChallengeSessions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Challenge",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Advisor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubSubjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionPdfs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionMain");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionExams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuestionClass");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HostCallLogging");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExamLectures");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Difficulty");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ConfigurationValues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChallengeTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChallengeTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChallengeTemplateItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ChallengeSessions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Challenge");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Advisor");
        }
    }
}
