namespace EduExamine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Chapter : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ChapterDates", name: "ChapterDateId", newName: "ChapterId");
            RenameIndex(table: "dbo.ChapterDates", name: "IX_ChapterDateId", newName: "IX_ChapterId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ChapterDates", name: "IX_ChapterId", newName: "IX_ChapterDateId");
            RenameColumn(table: "dbo.ChapterDates", name: "ChapterId", newName: "ChapterDateId");
        }
    }
}
