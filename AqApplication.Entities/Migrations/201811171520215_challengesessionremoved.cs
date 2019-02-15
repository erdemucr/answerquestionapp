namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengesessionremoved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChallengeQuestionAnswers", "ChallengeId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChallengeQuestionAnswers", "ChallengeId");
            AddForeignKey("dbo.ChallengeQuestionAnswers", "ChallengeId", "dbo.Challenges", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChallengeQuestionAnswers", "ChallengeId", "dbo.Challenges");
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "ChallengeId" });
            DropColumn("dbo.ChallengeQuestionAnswers", "ChallengeId");
        }
    }
}
