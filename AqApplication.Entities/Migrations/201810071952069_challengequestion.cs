namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengequestion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChallengeQuestions", "ChallengeSessionId", "dbo.ChallengeSessions");
            DropIndex("dbo.ChallengeQuestions", new[] { "ChallengeSessionId" });
            DropColumn("dbo.ChallengeQuestions", "ChallengeSessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChallengeQuestions", "ChallengeSessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChallengeQuestions", "ChallengeSessionId");
            AddForeignKey("dbo.ChallengeQuestions", "ChallengeSessionId", "dbo.ChallengeSessions", "Id", cascadeDelete: true);
        }
    }
}
