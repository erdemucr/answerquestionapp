namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class challengetitlefixed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Challenges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChallengeTypeId = c.Int(nullable: false),
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
                .ForeignKey("dbo.ChallengeTypes", t => t.ChallengeTypeId, cascadeDelete: true)
                .Index(t => t.ChallengeTypeId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.ChallengeQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seo = c.Int(nullable: false),
                        ChallengeSessionId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Challenge_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChallengeSessions", t => t.ChallengeSessionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionMains", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Challenges", t => t.Challenge_Id)
                .Index(t => t.ChallengeSessionId)
                .Index(t => t.QuestionId)
                .Index(t => t.Challenge_Id);
            
            CreateTable(
                "dbo.ChallengeSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Challenges", t => t.ChallengeId, cascadeDelete: true)
                .Index(t => t.ChallengeId)
                .Index(t => t.UserId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.ChallengeTypes",
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
                "dbo.ChallengeQuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        AnswerId = c.Int(nullable: false),
                        ChallengeSessionId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.ChallengeSessions", t => t.ChallengeSessionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionAnswers", t => t.AnswerId, cascadeDelete: true)
                .Index(t => t.AnswerId)
                .Index(t => t.ChallengeSessionId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChallengeQuestionAnswers", "AnswerId", "dbo.QuestionAnswers");
            DropForeignKey("dbo.ChallengeQuestionAnswers", "ChallengeSessionId", "dbo.ChallengeSessions");
            DropForeignKey("dbo.ChallengeQuestionAnswers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challenges", "ChallengeTypeId", "dbo.ChallengeTypes");
            DropForeignKey("dbo.ChallengeTypes", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallengeTypes", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallengeQuestions", "Challenge_Id", "dbo.Challenges");
            DropForeignKey("dbo.ChallengeQuestions", "QuestionId", "dbo.QuestionMains");
            DropForeignKey("dbo.ChallengeQuestions", "ChallengeSessionId", "dbo.ChallengeSessions");
            DropForeignKey("dbo.ChallengeSessions", "ChallengeId", "dbo.Challenges");
            DropForeignKey("dbo.ChallengeSessions", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallengeSessions", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChallengeSessions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challenges", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Challenges", "Creator", "dbo.AspNetUsers");
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "UserId" });
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "ChallengeSessionId" });
            DropIndex("dbo.ChallengeQuestionAnswers", new[] { "AnswerId" });
            DropIndex("dbo.ChallengeTypes", new[] { "Editor" });
            DropIndex("dbo.ChallengeTypes", new[] { "Creator" });
            DropIndex("dbo.ChallengeSessions", new[] { "Editor" });
            DropIndex("dbo.ChallengeSessions", new[] { "Creator" });
            DropIndex("dbo.ChallengeSessions", new[] { "UserId" });
            DropIndex("dbo.ChallengeSessions", new[] { "ChallengeId" });
            DropIndex("dbo.ChallengeQuestions", new[] { "Challenge_Id" });
            DropIndex("dbo.ChallengeQuestions", new[] { "QuestionId" });
            DropIndex("dbo.ChallengeQuestions", new[] { "ChallengeSessionId" });
            DropIndex("dbo.Challenges", new[] { "Editor" });
            DropIndex("dbo.Challenges", new[] { "Creator" });
            DropIndex("dbo.Challenges", new[] { "ChallengeTypeId" });
            DropTable("dbo.ChallengeQuestionAnswers");
            DropTable("dbo.ChallengeTypes");
            DropTable("dbo.ChallengeSessions");
            DropTable("dbo.ChallengeQuestions");
            DropTable("dbo.Challenges");
        }
    }
}
