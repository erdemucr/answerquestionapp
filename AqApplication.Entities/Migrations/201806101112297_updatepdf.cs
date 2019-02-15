namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionPdfContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionPdfs", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionPdfContents", "QuestionId", "dbo.QuestionPdfs");
            DropIndex("dbo.QuestionPdfContents", new[] { "QuestionId" });
            DropTable("dbo.QuestionPdfContents");
        }
    }
}
