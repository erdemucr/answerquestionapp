namespace AqApplication.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MemberType = c.Int(nullable: false),
                        TelNo = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Exams",
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
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        IsTrue = c.Boolean(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(),
                        Seo = c.Int(),
                        Creator = c.String(maxLength: 128),
                        Editor = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        QuestionMain_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator)
                .ForeignKey("dbo.AspNetUsers", t => t.Editor)
                .ForeignKey("dbo.QuestionMains", t => t.QuestionMain_Id)
                .Index(t => t.Creator)
                .Index(t => t.Editor)
                .Index(t => t.QuestionMain_Id);
            
            CreateTable(
                "dbo.QuestionClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionMains", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.ClassId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.QuestionMains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MainTitle = c.String(),
                        MainImage = c.String(),
                        SubSubjectId = c.Int(nullable: false),
                        Difficulty = c.Int(),
                        Offer = c.Boolean(),
                        Licence = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.SubSubjects", t => t.SubSubjectId, cascadeDelete: true)
                .Index(t => t.SubSubjectId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.QuestionExams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        ExamId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionMains", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.ExamId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.SubSubjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LectureId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Lectures", t => t.LectureId, cascadeDelete: true)
                .Index(t => t.LectureId)
                .Index(t => t.Creator)
                .Index(t => t.Editor);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuestionMains", "SubSubjectId", "dbo.SubSubjects");
            DropForeignKey("dbo.SubSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "LectureId", "dbo.Lectures");
            DropForeignKey("dbo.Subjects", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubSubjects", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubSubjects", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionExams", "QuestionId", "dbo.QuestionMains");
            DropForeignKey("dbo.QuestionExams", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.QuestionExams", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionExams", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionClasses", "QuestionId", "dbo.QuestionMains");
            DropForeignKey("dbo.QuestionAnswers", "QuestionMain_Id", "dbo.QuestionMains");
            DropForeignKey("dbo.QuestionMains", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionMains", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionClasses", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.QuestionClasses", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionClasses", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionAnswers", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.QuestionAnswers", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lectures", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Lectures", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.Exams", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Exams", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classes", "Editor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classes", "Creator", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Subjects", new[] { "Editor" });
            DropIndex("dbo.Subjects", new[] { "Creator" });
            DropIndex("dbo.Subjects", new[] { "LectureId" });
            DropIndex("dbo.SubSubjects", new[] { "Editor" });
            DropIndex("dbo.SubSubjects", new[] { "Creator" });
            DropIndex("dbo.SubSubjects", new[] { "SubjectId" });
            DropIndex("dbo.QuestionExams", new[] { "Editor" });
            DropIndex("dbo.QuestionExams", new[] { "Creator" });
            DropIndex("dbo.QuestionExams", new[] { "ExamId" });
            DropIndex("dbo.QuestionExams", new[] { "QuestionId" });
            DropIndex("dbo.QuestionMains", new[] { "Editor" });
            DropIndex("dbo.QuestionMains", new[] { "Creator" });
            DropIndex("dbo.QuestionMains", new[] { "SubSubjectId" });
            DropIndex("dbo.QuestionClasses", new[] { "Editor" });
            DropIndex("dbo.QuestionClasses", new[] { "Creator" });
            DropIndex("dbo.QuestionClasses", new[] { "ClassId" });
            DropIndex("dbo.QuestionClasses", new[] { "QuestionId" });
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionMain_Id" });
            DropIndex("dbo.QuestionAnswers", new[] { "Editor" });
            DropIndex("dbo.QuestionAnswers", new[] { "Creator" });
            DropIndex("dbo.Lectures", new[] { "Editor" });
            DropIndex("dbo.Lectures", new[] { "Creator" });
            DropIndex("dbo.Exams", new[] { "Editor" });
            DropIndex("dbo.Exams", new[] { "Creator" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Classes", new[] { "Editor" });
            DropIndex("dbo.Classes", new[] { "Creator" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Subjects");
            DropTable("dbo.SubSubjects");
            DropTable("dbo.QuestionExams");
            DropTable("dbo.QuestionMains");
            DropTable("dbo.QuestionClasses");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.Lectures");
            DropTable("dbo.Exams");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Classes");
        }
    }
}
