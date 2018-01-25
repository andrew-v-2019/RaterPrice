namespace RaterPrice.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                        Vendor = c.String(),
                        Barcode = c.String(),
                        IsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileUrl = c.String(),
                        ImageTypeId = c.Int(nullable: false),
                        Main = c.Boolean(nullable: false),
                        ObjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImageTypes", t => t.ImageTypeId, cascadeDelete: true)
                .Index(t => t.ImageTypeId);
            
            CreateTable(
                "dbo.ImageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ShopId = c.Int(nullable: false),
                        GoodId = c.Int(nullable: false),
                        PriceValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shops", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.Shops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        IsDraft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.UserRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phone = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInRoles", "RoleId", "dbo.UserRoles");
            DropForeignKey("dbo.UserInRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Prices", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Images", "ImageTypeId", "dbo.ImageTypes");
            DropIndex("dbo.UserInRoles", new[] { "RoleId" });
            DropIndex("dbo.UserInRoles", new[] { "UserId" });
            DropIndex("dbo.Prices", new[] { "ShopId" });
            DropIndex("dbo.Images", new[] { "ImageTypeId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Shops");
            DropTable("dbo.Prices");
            DropTable("dbo.ImageTypes");
            DropTable("dbo.Images");
            DropTable("dbo.Goods");
            DropTable("dbo.Cities");
        }
    }
}
