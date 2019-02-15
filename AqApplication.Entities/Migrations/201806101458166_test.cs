namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionPdfs", "TotalPage", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionPdfs", "PdfUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionPdfs", "PdfUrl");
            DropColumn("dbo.QuestionPdfs", "TotalPage");
        }
    }
}
