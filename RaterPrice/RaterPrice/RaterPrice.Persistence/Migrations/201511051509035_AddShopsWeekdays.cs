namespace RaterPrice.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShopsWeekdays : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.ShopWeekdays",
                   c =>
                       new
                       {
                           Id = c.Int(false, true),
                           ShopId = c.Int(false),
                           WeekdayId = c.Int(false),
                           StartWorkHour = c.Time(nullable: true, defaultValue: null),
                           EndWorkHour = c.Time(nullable: true, defaultValue: null),
                           DinnerBreakStartHour = c.Time(nullable: true, defaultValue: null),
                           DinnerBreakStopHour = c.Time(nullable: true, defaultValue: null)
                       }).PrimaryKey(c => c.Id);
            AddForeignKey("dbo.ShopWeekdays", "ShopId", "Shops", "Id");
            AddForeignKey("dbo.ShopWeekdays", "WeekdayId", "Weekdays", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShopWeekdays", "ShopId", "Shops");
            DropForeignKey("dbo.ShopWeekdays", "WeekdayId", "Weekdays");
            DropTable("dbo.ShopWeekdays");
        }
    }
}
