namespace RaterPrice.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWeekdays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.Weekdays",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Name = c.String(),
               })
               .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Weekdays");
        }
    }
}
