namespace EduExamine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Admins", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Admins", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Chapters", "ChapterName", c => c.String(nullable: false));
            AlterColumn("dbo.EduYears", "EduYearName", c => c.String(nullable: false));
            AlterColumn("dbo.Exams", "ExamName", c => c.String(nullable: false));
            AlterColumn("dbo.Subjects", "SubjectName", c => c.String(nullable: false));
            AlterColumn("dbo.Classes", "ClassName", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "LoginName", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.TeachingTypes", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeachingTypes", "Name", c => c.String());
            AlterColumn("dbo.Teachers", "Password", c => c.String());
            AlterColumn("dbo.Teachers", "LoginName", c => c.String());
            AlterColumn("dbo.Teachers", "FullName", c => c.String());
            AlterColumn("dbo.Classes", "ClassName", c => c.String());
            AlterColumn("dbo.Subjects", "SubjectName", c => c.String());
            AlterColumn("dbo.Exams", "ExamName", c => c.String());
            AlterColumn("dbo.EduYears", "EduYearName", c => c.String());
            AlterColumn("dbo.Chapters", "ChapterName", c => c.String());
            AlterColumn("dbo.Admins", "Password", c => c.String());
            AlterColumn("dbo.Admins", "Name", c => c.String());
        }
    }
}
