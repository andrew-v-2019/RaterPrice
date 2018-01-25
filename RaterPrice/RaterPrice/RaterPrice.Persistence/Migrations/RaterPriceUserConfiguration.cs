using RaterPrice.Persistence.Domain;
using System.Data.Entity.ModelConfiguration;

namespace RaterPrice.Persistence.Migrations
{
    class RaterPriceUserConfiguration: EntityTypeConfiguration<User>
    {
        public RaterPriceUserConfiguration()
        {
            ToTable("Users");
        }
    }
}
