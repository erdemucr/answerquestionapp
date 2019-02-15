namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChallengeAnswerFixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChallengeQuestionAnswers", "AnswerId", "dbo.QuestionAnswers");
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "AnswerId" });
            AddColumn("dbo.ChallengeQuestionAnswers", "AnswerIndex", c => c.Int(nullable: false));
            DropColumn("dbo.ChallengeQuestionAnswers", "AnswerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChallengeQuestionAnswers", "AnswerId", c => c.Int(nullable: false));
            DropColumn("dbo.ChallengeQuestionAnswers", "AnswerIndex");
            CreateIndex("dbo.ChallengeQuestionAnswers", "AnswerId");
            AddForeignKey("dbo.ChallengeQuestionAnswers", "AnswerId", "dbo.QuestionAnswers", "Id", cascadeDelete: true);
        }
    }
}
