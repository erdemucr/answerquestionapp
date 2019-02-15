namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pdf2modfication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionPdfs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Seo = c.Int(),
                        Creator = c.String(maxLength: 128),
                        Editor = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator)
                .ForeignKey("dbo.AspNetUsers", t => t.Editor)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            AddColumn("dbo.QuestionMains", "QuestionPdfId", c => c.Int());
            CreateIndex("dbo.QuestionMains", "QuestionPdfId");
            AddForeignKey("dbo.QuestionMains", "QuestionPdfId", "dbo.QuestionPdfs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionMains", "QuestionPdfId", "dbo.QuestionPdfs");
            DropForeignKey("dbo.QuestionPdfs", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionPdfs", "Creator", "dbo.AspNetUsers");
            DropIndex("dbo.QuestionPdfs", new[] { "Editor" });
            DropIndex("dbo.QuestionPdfs", new[] { "Creator" });
            DropIndex("dbo.QuestionMains", new[] { "QuestionPdfId" });
            DropColumn("dbo.QuestionMains", "QuestionPdfId");
            DropTable("dbo.QuestionPdfs");
        }
    }
}
