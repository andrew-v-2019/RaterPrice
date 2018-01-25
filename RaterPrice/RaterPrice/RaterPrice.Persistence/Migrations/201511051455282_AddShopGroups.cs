namespace RaterPrice.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShopGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.ShopGroups",
              c =>
                  new
                  {
                      Id = c.Int(false, true),
                      Name = c.String(false)
                  }).PrimaryKey(c => c.Id);


        }

        public override void Down()
        {
            DropTable("dbo.ShopGroups");
        }
    }
}
