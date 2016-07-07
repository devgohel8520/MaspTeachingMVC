namespace EduExamine.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class StatusforChapterTEaching : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChapterTeachings", "Status", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ChapterTeachings", "Status");
        }
    }
}
