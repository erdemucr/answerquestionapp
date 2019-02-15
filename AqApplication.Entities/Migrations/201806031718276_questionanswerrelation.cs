namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionanswerrelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionAnswers", "QuestionMain_Id", "dbo.QuestionMains");
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionMain_Id" });
            DropColumn("dbo.QuestionAnswers", "QuestionId");
            RenameColumn(table: "dbo.QuestionAnswers", name: "QuestionMain_Id", newName: "QuestionId");
            AlterColumn("dbo.QuestionAnswers", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionAnswers", "QuestionId");
            AddForeignKey("dbo.QuestionAnswers", "QuestionId", "dbo.QuestionMains", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionAnswers", "QuestionId", "dbo.QuestionMains");
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionId" });
            AlterColumn("dbo.QuestionAnswers", "QuestionId", c => c.Int());
            RenameColumn(table: "dbo.QuestionAnswers", name: "QuestionId", newName: "QuestionMain_Id");
            AddColumn("dbo.QuestionAnswers", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionAnswers", "QuestionMain_Id");
            AddForeignKey("dbo.QuestionAnswers", "QuestionMain_Id", "dbo.QuestionMains", "Id");
        }
    }
}
