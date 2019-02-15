namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengeremoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Challanges", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challanges", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeTypes", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeTypes", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challanges", "ChallangeTypeId", "dbo.ChallangeTypes");
            DropForeignKey("dbo.ChallangeSessions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeSessions", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeSessions", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallangeSessions", "ChallangeId", "dbo.Challanges");
            DropForeignKey("dbo.ChallangeQuizs", "ChallangeSessionId", "dbo.ChallangeSessions");
            DropForeignKey("dbo.ChallangeQuizs", "QuestionId", "dbo.QuestionMains");
            DropForeignKey("dbo.UserQuestions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserQuestions", "ChallangeSessionId", "dbo.ChallangeSessions");
            DropForeignKey("dbo.UserQuestions", "AnswerId", "dbo.QuestionAnswers");
            DropIndex("dbo.Challanges", new[] { "ChallangeTypeId" });
            DropIndex("dbo.Challanges", new[] { "Creator" });
            DropIndex("dbo.Challanges", new[] { "Editor" });
            DropIndex("dbo.ChallangeTypes", new[] { "Creator" });
            DropIndex("dbo.ChallangeTypes", new[] { "Editor" });
            DropIndex("dbo.ChallangeQuizs", new[] { "ChallangeSessionId" });
            DropIndex("dbo.ChallangeQuizs", new[] { "QuestionId" });
            DropIndex("dbo.ChallangeSessions", new[] { "ChallangeId" });
            DropIndex("dbo.ChallangeSessions", new[] { "UserId" });
            DropIndex("dbo.ChallangeSessions", new[] { "Creator" });
            DropIndex("dbo.ChallangeSessions", new[] { "Editor" });
            DropIndex("dbo.UserQuestions", new[] { "AnswerId" });
            DropIndex("dbo.UserQuestions", new[] { "ChallangeSessionId" });
            DropIndex("dbo.UserQuestions", new[] { "UserId" });
            DropTable("dbo.Challanges");
            DropTable("dbo.ChallangeTypes");
            DropTable("dbo.ChallangeQuizs");
            DropTable("dbo.ChallangeSessions");
            DropTable("dbo.UserQuestions");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChallangeQuizs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seo = c.Int(nullable: false),
                        ChallangeSessionId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserQuestions", "UserId");
            CreateIndex("dbo.UserQuestions", "ChallangeSessionId");
            CreateIndex("dbo.UserQuestions", "AnswerId");
            CreateIndex("dbo.ChallangeSessions", "Editor");
            CreateIndex("dbo.ChallangeSessions", "Creator");
            CreateIndex("dbo.ChallangeSessions", "UserId");
            CreateIndex("dbo.ChallangeSessions", "ChallangeId");
            CreateIndex("dbo.ChallangeQuizs", "QuestionId");
            CreateIndex("dbo.ChallangeQuizs", "ChallangeSessionId");
            CreateIndex("dbo.ChallangeTypes", "Editor");
            CreateIndex("dbo.ChallangeTypes", "Creator");
            CreateIndex("dbo.Challanges", "Editor");
            CreateIndex("dbo.Challanges", "Creator");
            CreateIndex("dbo.Challanges", "ChallangeTypeId");
            AddForeignKey("dbo.UserQuestions", "AnswerId", "dbo.QuestionAnswers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserQuestions", "ChallangeSessionId", "dbo.ChallangeSessions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserQuestions", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChallangeQuizs", "QuestionId", "dbo.QuestionMains", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChallangeQuizs", "ChallangeSessionId", "dbo.ChallangeSessions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChallangeSessions", "ChallangeId", "dbo.Challanges", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChallangeSessions", "Editor", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChallangeSessions", "Creator", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChallangeSessions", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Challanges", "ChallangeTypeId", "dbo.ChallangeTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChallangeTypes", "Editor", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.ChallangeTypes", "Creator", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Challanges", "Editor", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Challanges", "Creator", "dbo.AspNetUsers", "Id");
        }
    }
}
