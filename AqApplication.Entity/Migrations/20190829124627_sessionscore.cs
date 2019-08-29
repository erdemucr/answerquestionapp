using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class sessionscore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectCount",
                table: "ChallengeSessions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalScore",
                table: "ChallengeSessions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectCount",
                table: "ChallengeSessions");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "ChallengeSessions");
        }
    }
}
