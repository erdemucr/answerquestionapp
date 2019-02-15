namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionanswerfixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionAnswers", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionAnswers", "Editor", "dbo.AspNetUsers");
            DropIndex("dbo.QuestionAnswers", new[] { "Creator" });
            DropIndex("dbo.QuestionAnswers", new[] { "Editor" });
            DropColumn("dbo.QuestionAnswers", "Name");
            DropColumn("dbo.QuestionAnswers", "CreatedDate");
            DropColumn("dbo.QuestionAnswers", "ModifiedDate");
            DropColumn("dbo.QuestionAnswers", "Seo");
            DropColumn("dbo.QuestionAnswers", "Creator");
            DropColumn("dbo.QuestionAnswers", "Editor");
            DropColumn("dbo.QuestionAnswers", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionAnswers", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuestionAnswers", "Editor", c => c.String(maxLength: 128));
            AddColumn("dbo.QuestionAnswers", "Creator", c => c.String(maxLength: 128));
            AddColumn("dbo.QuestionAnswers", "Seo", c => c.Int());
            AddColumn("dbo.QuestionAnswers", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.QuestionAnswers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.QuestionAnswers", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.QuestionAnswers", "Editor");
            CreateIndex("dbo.QuestionAnswers", "Creator");
            AddForeignKey("dbo.QuestionAnswers", "Editor", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.QuestionAnswers", "Creator", "dbo.AspNetUsers", "Id");
        }
    }
}
