namespace RaterPrice.Persistence.Migrations
{
      using System.Data.Entity.Migrations;
    
    public partial class AddShopPaymentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Shops", "GroupId", p => p.Int(nullable: true));
            AddColumn("Shops", "SubGroupId", p => p.Int(nullable: true));

            AddForeignKey("Shops", "GroupId", "ShopGroups", "Id");
            AddForeignKey("Shops", "SubGroupId", "ShopGroups", "Id");
        }

        public override void Down()
        {
            DropForeignKey("Shops", "GroupId", "ShopGroups");
            DropForeignKey("Shops", "SubGroupId", "ShopGroups");

            DropColumn("Shops", "GroupId");
            DropColumn("Shops", "SubGroupId");
        }
    }
}
