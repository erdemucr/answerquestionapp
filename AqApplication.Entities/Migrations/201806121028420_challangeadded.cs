namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challangeadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Challanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChallangeTypeId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        TimePeriod = c.Int(),
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
                .ForeignKey("dbo.ChallangeTypes", t => t.ChallangeTypeId, cascadeDelete: true)
                .Index(t => t.ChallangeTypeId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.ChallangeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 30),
                        Description = c.String(maxLength: 250),
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
            
            CreateTable(
                "dbo.ChallangeQuizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seo = c.Int(nullable: false),
                        ChallangeSessionId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChallangeSessions", t => t.ChallangeSessionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionMains", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.ChallangeSessionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.ChallangeSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChallangeId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Seo = c.Int(),
                        Creator = c.String(maxLength: 128),
                        Editor = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator)
                .ForeignKey("dbo.AspNetUsers", t => t.Editor)
                .ForeignKey("dbo.Challanges", t => t.ChallangeId, cascadeDelete: true)
                .Index(t => t.ChallangeId)
                .Index(t => t.UserId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.UserQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        AnswerId = c.Int(nullable: false),
                        ChallangeSessionId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.ChallangeSessions", t => t.ChallangeSessionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionAnswers", t => t.AnswerId, cascadeDelete: true)
                .Index(t => t.AnswerId)
                .Index(t => t.ChallangeSessionId)
                .Index(t => t.UserId);
            
            DropColumn("dbo.QuestionMains", "Name");
            DropColumn("dbo.QuestionClasses", "Name");
            DropColumn("dbo.QuestionExams", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionExams", "Name", c => c.String(nullable: false));
            AddColumn("dbo.QuestionClasses", "Name", c => c.String(nullable: false));
            AddColumn("dbo.QuestionMains", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.UserQuestions", "AnswerId", "dbo.QuestionAnswers");
            DropForeignKey("dbo.UserQuestions", "ChallangeSessionId", "dbo.ChallangeSessions");
            DropForeignKey("dbo.UserQuestions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeQuizs", "QuestionId", "dbo.QuestionMains");
            DropForeignKey("dbo.ChallangeQuizs", "ChallangeSessionId", "dbo.ChallangeSessions");
            DropForeignKey("dbo.ChallangeSessions", "ChallangeId", "dbo.Challanges");
            DropForeignKey("dbo.ChallangeSessions", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeSessions", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeSessions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challanges", "ChallangeTypeId", "dbo.ChallangeTypes");
            DropForeignKey("dbo.ChallangeTypes", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeTypes", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challanges", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challanges", "Creator", "dbo.AspNetUsers");
            DropIndex("dbo.UserQuestions", new[] { "UserId" });
            DropIndex("dbo.UserQuestions", new[] { "ChallangeSessionId" });
            DropIndex("dbo.UserQuestions", new[] { "AnswerId" });
            DropIndex("dbo.ChallangeSessions", new[] { "Editor" });
            DropIndex("dbo.ChallangeSessions", new[] { "Creator" });
            DropIndex("dbo.ChallangeSessions", new[] { "UserId" });
            DropIndex("dbo.ChallangeSessions", new[] { "ChallangeId" });
            DropIndex("dbo.ChallangeQuizs", new[] { "QuestionId" });
            DropIndex("dbo.ChallangeQuizs", new[] { "ChallangeSessionId" });
            DropIndex("dbo.ChallangeTypes", new[] { "Editor" });
            DropIndex("dbo.ChallangeTypes", new[] { "Creator" });
            DropIndex("dbo.Challanges", new[] { "Editor" });
            DropIndex("dbo.Challanges", new[] { "Creator" });
            DropIndex("dbo.Challanges", new[] { "ChallangeTypeId" });
            DropTable("dbo.UserQuestions");
            DropTable("dbo.ChallangeSessions");
            DropTable("dbo.ChallangeQuizs");
            DropTable("dbo.ChallangeTypes");
            DropTable("dbo.Challanges");
        }
    }
}
