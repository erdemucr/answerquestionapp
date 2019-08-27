using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AqApplication.Entity.Migrations
{
    public partial class challegeTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChallengeTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Seo = table.Column<int>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Editor = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplates_AspNetUsers_Creator",
                        column: x => x.Creator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplates_AspNetUsers_Editor",
                        column: x => x.Editor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeTemplateItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Seo = table.Column<int>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    Editor = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    Difficulty = table.Column<int>(nullable: true),
                    SubSubjectId = table.Column<int>(nullable: true),
                    SubjectId = table.Column<int>(nullable: true),
                    QuestionPdfId = table.Column<int>(nullable: true),
                    ChallengeTemplateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeTemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_ChallengeTemplates_ChallengeTemplateId",
                        column: x => x.ChallengeTemplateId,
                        principalTable: "ChallengeTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_AspNetUsers_Creator",
                        column: x => x.Creator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_AspNetUsers_Editor",
                        column: x => x.Editor,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_QuestionPdfs_QuestionPdfId",
                        column: x => x.QuestionPdfId,
                        principalTable: "QuestionPdfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_SubSubjects_SubSubjectId",
                        column: x => x.SubSubjectId,
                        principalTable: "SubSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeTemplateItems_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_ChallengeTemplateId",
                table: "ChallengeTemplateItems",
                column: "ChallengeTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_Creator",
                table: "ChallengeTemplateItems",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_Editor",
                table: "ChallengeTemplateItems",
                column: "Editor");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_ExamId",
                table: "ChallengeTemplateItems",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_QuestionPdfId",
                table: "ChallengeTemplateItems",
                column: "QuestionPdfId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_SubSubjectId",
                table: "ChallengeTemplateItems",
                column: "SubSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplateItems_SubjectId",
                table: "ChallengeTemplateItems",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplates_Creator",
                table: "ChallengeTemplates",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeTemplates_Editor",
                table: "ChallengeTemplates",
                column: "Editor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeTemplateItems");

            migrationBuilder.DropTable(
                name: "ChallengeTemplates");
        }
    }
}
