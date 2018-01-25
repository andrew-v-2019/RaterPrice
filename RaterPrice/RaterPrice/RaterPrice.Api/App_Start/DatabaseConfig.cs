using System.Configuration;
using Migrator;
using RaterPrice.Api.Infrastructure;

namespace RaterPrice.Api
{
    public static class DatabaseConfig
    {
        public static void SetupDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            EntityFrameworkMigrator.MigrateToLatest<RaterPrice.Persistence.Migrations.Configuration>(connectionString);

            
          

        }
    }
}