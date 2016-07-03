namespace EduExamine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseScripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                        Types = c.Int(nullable: false),
                        EduYearId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("dbo.EduYears", t => t.EduYearId, cascadeDelete: true)
                .Index(t => t.EduYearId);
            
            CreateTable(
                "dbo.AdminProfiles",
                c => new
                    {
                        AdminId = c.Long(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Mobile = c.String(),
                        Phone = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        PinCode = c.String(),
                    })
                .PrimaryKey(t => t.AdminId)
                .ForeignKey("dbo.Admins", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.EduYears",
                c => new
                    {
                        EduYearId = c.Int(nullable: false, identity: true),
                        EduYearName = c.String(),
                        EduStart = c.DateTime(nullable: false),
                        EduEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EduYearId);
            
            CreateTable(
                "dbo.ChapterDates",
                c => new
                    {
                        ChapterDateId = c.Long(nullable: false),
                        CStartDate = c.DateTime(nullable: false),
                        CEndDate = c.DateTime(nullable: false),
                        EduYearId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChapterDateId)
                .ForeignKey("dbo.Chapters", t => t.ChapterDateId)
                .ForeignKey("dbo.EduYears", t => t.EduYearId, cascadeDelete: true)
                .Index(t => t.ChapterDateId)
                .Index(t => t.EduYearId);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ChapterId = c.Long(nullable: false, identity: true),
                        ChapterName = c.String(),
                        SubjectId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ChapterId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.ChapterTeachings",
                c => new
                    {
                        ChapterTeachingId = c.Long(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        TeachingTypeId = c.Int(nullable: false),
                        MaxVal = c.Int(nullable: false),
                        MinVal = c.Int(nullable: false),
                        EduYearId = c.Int(nullable: false),
                        ChapterId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ChapterTeachingId)
                .ForeignKey("dbo.Chapters", t => t.ChapterId, cascadeDelete: true)
                .ForeignKey("dbo.EduYears", t => t.EduYearId, cascadeDelete: true)
                .ForeignKey("dbo.TeachingTypes", t => t.TeachingTypeId, cascadeDelete: true)
                .Index(t => t.TeachingTypeId)
                .Index(t => t.EduYearId)
                .Index(t => t.ChapterId);
            
            CreateTable(
                "dbo.TeachingTypes",
                c => new
                    {
                        TeachingTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeachingTypeId);
            
            CreateTable(
                "dbo.TeacherTeachings",
                c => new
                    {
                        TeacherTeachingId = c.Long(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        TeachingTypeId = c.Int(nullable: false),
                        MaxVal = c.Int(nullable: false),
                        MinVal = c.Int(nullable: false),
                        Marks = c.Int(nullable: false),
                        ChapterId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherTeachingId)
                .ForeignKey("dbo.Chapters", t => t.ChapterId, cascadeDelete: true)
                .ForeignKey("dbo.TeachingTypes", t => t.TeachingTypeId, cascadeDelete: true)
                .Index(t => t.TeachingTypeId)
                .Index(t => t.ChapterId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Long(nullable: false, identity: true),
                        SubjectName = c.String(),
                        ClassesId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Classes", t => t.ClassesId, cascadeDelete: true)
                .Index(t => t.ClassesId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassesId = c.Long(nullable: false, identity: true),
                        ClassName = c.String(),
                    })
                .PrimaryKey(t => t.ClassesId);
            
            CreateTable(
                "dbo.ExamSubjects",
                c => new
                    {
                        ExamSubjectId = c.Long(nullable: false, identity: true),
                        SubjectId = c.Long(nullable: false),
                        AvgMarks = c.Int(nullable: false),
                        ExamMarks = c.Int(nullable: false),
                        ExamId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ExamSubjectId)
                .ForeignKey("dbo.Exams", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ExamId);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamId = c.Long(nullable: false, identity: true),
                        ExamName = c.String(),
                        EduYearId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamId)
                .ForeignKey("dbo.EduYears", t => t.EduYearId, cascadeDelete: true)
                .Index(t => t.EduYearId);
            
            CreateTable(
                "dbo.TeacherSubjects",
                c => new
                    {
                        TeacherSubjectId = c.Long(nullable: false, identity: true),
                        AdminId = c.Long(nullable: false),
                        SubjectId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherSubjectId)
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.AdminId)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.TeacherChapterDates",
                c => new
                    {
                        TeacherChapterDateId = c.Long(nullable: false),
                        AdminId = c.Long(nullable: false),
                        TCStartDate = c.DateTime(nullable: false),
                        TCEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherChapterDateId)
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: true)
                .ForeignKey("dbo.ChapterDates", t => t.TeacherChapterDateId)
                .Index(t => t.TeacherChapterDateId)
                .Index(t => t.AdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Admins", "EduYearId", "dbo.EduYears");
            DropForeignKey("dbo.TeacherChapterDates", "TeacherChapterDateId", "dbo.ChapterDates");
            DropForeignKey("dbo.TeacherChapterDates", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ChapterDates", "EduYearId", "dbo.EduYears");
            DropForeignKey("dbo.ChapterDates", "ChapterDateId", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.TeacherSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.TeacherSubjects", "AdminId", "dbo.Admins");
            DropForeignKey("dbo.ExamSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ExamSubjects", "ExamId", "dbo.Exams");
            DropForeignKey("dbo.Exams", "EduYearId", "dbo.EduYears");
            DropForeignKey("dbo.Subjects", "ClassesId", "dbo.Classes");
            DropForeignKey("dbo.ChapterTeachings", "TeachingTypeId", "dbo.TeachingTypes");
            DropForeignKey("dbo.TeacherTeachings", "TeachingTypeId", "dbo.TeachingTypes");
            DropForeignKey("dbo.TeacherTeachings", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.ChapterTeachings", "EduYearId", "dbo.EduYears");
            DropForeignKey("dbo.ChapterTeachings", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.AdminProfiles", "AdminId", "dbo.Admins");
            DropIndex("dbo.TeacherChapterDates", new[] { "AdminId" });
            DropIndex("dbo.TeacherChapterDates", new[] { "TeacherChapterDateId" });
            DropIndex("dbo.TeacherSubjects", new[] { "SubjectId" });
            DropIndex("dbo.TeacherSubjects", new[] { "AdminId" });
            DropIndex("dbo.Exams", new[] { "EduYearId" });
            DropIndex("dbo.ExamSubjects", new[] { "ExamId" });
            DropIndex("dbo.ExamSubjects", new[] { "SubjectId" });
            DropIndex("dbo.Subjects", new[] { "ClassesId" });
            DropIndex("dbo.TeacherTeachings", new[] { "ChapterId" });
            DropIndex("dbo.TeacherTeachings", new[] { "TeachingTypeId" });
            DropIndex("dbo.ChapterTeachings", new[] { "ChapterId" });
            DropIndex("dbo.ChapterTeachings", new[] { "EduYearId" });
            DropIndex("dbo.ChapterTeachings", new[] { "TeachingTypeId" });
            DropIndex("dbo.Chapters", new[] { "SubjectId" });
            DropIndex("dbo.ChapterDates", new[] { "EduYearId" });
            DropIndex("dbo.ChapterDates", new[] { "ChapterDateId" });
            DropIndex("dbo.AdminProfiles", new[] { "AdminId" });
            DropIndex("dbo.Admins", new[] { "EduYearId" });
            DropTable("dbo.TeacherChapterDates");
            DropTable("dbo.TeacherSubjects");
            DropTable("dbo.Exams");
            DropTable("dbo.ExamSubjects");
            DropTable("dbo.Classes");
            DropTable("dbo.Subjects");
            DropTable("dbo.TeacherTeachings");
            DropTable("dbo.TeachingTypes");
            DropTable("dbo.ChapterTeachings");
            DropTable("dbo.Chapters");
            DropTable("dbo.ChapterDates");
            DropTable("dbo.EduYears");
            DropTable("dbo.AdminProfiles");
            DropTable("dbo.Admins");
        }
    }
}
