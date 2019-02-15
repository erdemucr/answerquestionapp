namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChallengeQuestionAnswers", "TimeInterval", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChallengeQuestionAnswers", "TimeInterval");
        }
    }
}
