namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengequestion1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChallengeQuestionAnswers", "ChallengeSessionId", "dbo.ChallengeSessions");
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "ChallengeSessionId" });
            DropColumn("dbo.ChallengeQuestionAnswers", "ChallengeSessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChallengeQuestionAnswers", "ChallengeSessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChallengeQuestionAnswers", "ChallengeSessionId");
            AddForeignKey("dbo.ChallengeQuestionAnswers", "ChallengeSessionId", "dbo.ChallengeSessions", "Id", cascadeDelete: true);
        }
    }
}
