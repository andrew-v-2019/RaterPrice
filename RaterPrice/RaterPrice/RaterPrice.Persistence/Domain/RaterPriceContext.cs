
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace RaterPrice.Persistence.Domain
{
    public class RaterPriceContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public RaterPriceContext(): base("DefaultConnection")
        {
            // System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<RaterPriceContext, GoodsMigrationConfiguration>());
        }

        public RaterPriceContext(string connectionString) : base(connectionString)
        {
            
            // System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<RaterPriceContext, GoodsMigrationConfiguration>());
        }


        public static RaterPriceContext Create()
        {
            return new RaterPriceContext();
        }

        public DbSet<City> Cities { get; set; } // City
        public DbSet<Good> Goods { get; set; } // Goods
        public DbSet<Image> Images { get; set; } // Image
        public DbSet<ImageType> ImageTypes { get; set; } // ImageType
        public DbSet<Price> Prices { get; set; } // Price
        public DbSet<Shop> Shops { get; set; } // Shop


       // public DbSet<Role> Roles { get; set; } // UserRoles

        public DbSet<UserLogin> UserLogins { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }
       


        public DbSet<Weekday> Weekdays { get; set; }

        public DbSet<ShopGroup> ShopGroups { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<ShopPaymentType> ShopPaymentTypes { get; set; }

        public DbSet<ShopWeekDay> ShopWeekdays { get; set; }

        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }

        public DbSet<SmsSender> SmsSenders { get; set; }

        public DbSet<SmsMessage> SmsMessages { get; set; }

        public DbSet<SmsMessageSend> SmsMessageSends { get; set; }
    }
}
