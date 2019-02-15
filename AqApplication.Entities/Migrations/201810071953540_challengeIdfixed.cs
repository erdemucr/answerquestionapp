namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengeIdfixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChallengeQuestions", "Challenge_Id", "dbo.Challenges");
            DropIndex("dbo.ChallengeQuestions", new[] { "Challenge_Id" });
            RenameColumn(table: "dbo.ChallengeQuestions", name: "Challenge_Id", newName: "ChallengeId");
            AlterColumn("dbo.ChallengeQuestions", "ChallengeId", c => c.Int(nullable: false));
            CreateIndex("dbo.ChallengeQuestions", "ChallengeId");
            AddForeignKey("dbo.ChallengeQuestions", "ChallengeId", "dbo.Challenges", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChallengeQuestions", "ChallengeId", "dbo.Challenges");
            DropIndex("dbo.ChallengeQuestions", new[] { "ChallengeId" });
            AlterColumn("dbo.ChallengeQuestions", "ChallengeId", c => c.Int());
            RenameColumn(table: "dbo.ChallengeQuestions", name: "ChallengeId", newName: "Challenge_Id");
            CreateIndex("dbo.ChallengeQuestions", "Challenge_Id");
            AddForeignKey("dbo.ChallengeQuestions", "Challenge_Id", "dbo.Challenges", "Id");
        }
    }
}
