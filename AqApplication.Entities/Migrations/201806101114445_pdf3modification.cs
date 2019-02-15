namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pdf3modification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionPdfContents", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionPdfContents", "ImageUrl");
        }
    }
}
