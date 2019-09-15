using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class challengeQuestionTemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeQuestionsTemp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Seo = table.Column<int>(nullable: false),
                    ChallengeId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeQuestionsTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestionsTemp_Challenge_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeQuestionsTemp_QuestionMain_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionsTemp_ChallengeId",
                table: "ChallengeQuestionsTemp",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeQuestionsTemp_QuestionId",
                table: "ChallengeQuestionsTemp",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeQuestionsTemp");
        }
    }
}
