namespace EduExamine.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PercentageforExamSubject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamSubjects", "Percentages", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ExamSubjects", "Percentages");
        }
    }
}
