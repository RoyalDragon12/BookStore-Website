namespace WebApplication2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(),
                        isHidden = c.Int(nullable: false),
                        isHiddenBool = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Book_Genre_Junction",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.GenreId })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        BookName = c.String(nullable: false),
                        BookCode = c.String(),
                        Cost = c.Single(nullable: false),
                        Description = c.String(),
                        Image = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        StorageAmount = c.Int(nullable: false),
                        isHidden = c.Int(nullable: false),
                        isHiddenBool = c.Boolean(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Publishers", t => t.PublisherId, cascadeDelete: true)
                .Index(t => t.PublisherId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        PublisherId = c.Int(nullable: false, identity: true),
                        PublisherName = c.String(),
                        PublisherCode = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                        isHidden = c.Int(nullable: false),
                        isHiddenBool = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PublisherId);
            
            CreateTable(
                "dbo.CartInfoes",
                c => new
                    {
                        CartInfoId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        SingleCost = c.Single(nullable: false),
                        TotalCost = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.CartInfoId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.CartId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Paid = c.Int(nullable: false),
                        PaidBool = c.Boolean(nullable: false),
                        Completed = c.Int(nullable: false),
                        ShipmentProgress = c.Int(nullable: false),
                        ShippingAddress = c.String(),
                        ShippingCost = c.Single(nullable: false),
                        TotalCost = c.Single(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        CartNote = c.String(),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 10),
                        Password = c.String(nullable: false, maxLength: 15),
                        Email = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Roles = c.String(),
                        isBanned = c.Int(nullable: false),
                        isBannedBool = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                        GenreCode = c.String(),
                        isHidden = c.Int(nullable: false),
                        isHiddenBool = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenreId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotiDescription = c.String(),
                        NotiState = c.Int(nullable: false),
                        NotiDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PromotionDetails",
                c => new
                    {
                        PromotionDetailId = c.Int(nullable: false, identity: true),
                        DiscountedCost = c.Single(nullable: false),
                        BookId = c.Int(nullable: false),
                        PromotionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PromotionDetailId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Promotions", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.PromotionId);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        PromotionId = c.Int(nullable: false, identity: true),
                        PromoName = c.String(),
                        PromoDesc = c.String(),
                        PromoStartDate = c.DateTime(nullable: false),
                        PromoEndDate = c.DateTime(nullable: false),
                        PromoState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionDetails", "PromotionId", "dbo.Promotions");
            DropForeignKey("dbo.PromotionDetails", "BookId", "dbo.Books");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Book_Genre_Junction", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.Carts", "UserId", "dbo.Users");
            DropForeignKey("dbo.CartInfoes", "CartId", "dbo.Carts");
            DropForeignKey("dbo.CartInfoes", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Book_Genre_Junction", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "AuthorId", "dbo.Authors");
            DropIndex("dbo.PromotionDetails", new[] { "PromotionId" });
            DropIndex("dbo.PromotionDetails", new[] { "BookId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.CartInfoes", new[] { "CartId" });
            DropIndex("dbo.CartInfoes", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "AuthorId" });
            DropIndex("dbo.Books", new[] { "PublisherId" });
            DropIndex("dbo.Book_Genre_Junction", new[] { "GenreId" });
            DropIndex("dbo.Book_Genre_Junction", new[] { "BookId" });
            DropTable("dbo.Promotions");
            DropTable("dbo.PromotionDetails");
            DropTable("dbo.Notifications");
            DropTable("dbo.Genres");
            DropTable("dbo.Users");
            DropTable("dbo.Carts");
            DropTable("dbo.CartInfoes");
            DropTable("dbo.Publishers");
            DropTable("dbo.Books");
            DropTable("dbo.Book_Genre_Junction");
            DropTable("dbo.Authors");
        }
    }
}
