namespace RaterPrice.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ShopPaymentType2 : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.ShopPaymentTypes",
                c =>
                    new
                    {
                        Id = c.Int(false, true),
                        ShopId = c.Int(false),
                        PaymentTypeId = c.Int(false),

                    }).PrimaryKey(c => c.Id);
            AddForeignKey("ShopPaymentTypes", "ShopId", "Shops", "Id");
            AddForeignKey("ShopPaymentTypes", "PaymentTypeId", "PaymentTypes", "Id");


            Sql(@"
                    INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Наличный')
                    INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Безналичный')
                    INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('American Express')
                     INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Visa')
 INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Золотая корона')
INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Оплата через интернет')
                    INSERT [dbo].[PaymentTypes] ([Name]) VALUES ('Mastercard')");
        }
        
        public override void Down()
        {
            DropForeignKey("ShopPaymentTypes", "ShopId", "Shops");
            DropForeignKey("ShopPaymentTypes", "PaymentTypeId", "PaymentTypes");
            DropTable("dbo.ShopPaymentTypes");
        }
    }
}
