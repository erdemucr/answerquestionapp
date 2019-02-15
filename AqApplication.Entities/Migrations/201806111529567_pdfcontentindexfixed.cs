namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pdfcontentindexfixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionPdfContents", "Seo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionPdfContents", "Seo");
        }
    }
}
