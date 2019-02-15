namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Answercount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionMains", "AnswerCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionMains", "AnswerCount");
        }
    }
}
