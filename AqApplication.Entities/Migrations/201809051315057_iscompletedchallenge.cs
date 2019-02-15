namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iscompletedchallenge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChallengeSessions", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChallengeSessions", "IsCompleted");
        }
    }
}
