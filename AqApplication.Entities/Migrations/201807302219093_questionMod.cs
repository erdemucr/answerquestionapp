namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionMod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionMains", "CorrectAnswer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionMains", "CorrectAnswer");
        }
    }
}
