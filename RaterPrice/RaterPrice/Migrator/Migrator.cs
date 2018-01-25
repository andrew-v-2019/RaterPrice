using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace Migrator
{
    public static class EntityFrameworkMigrator
    {
        public static void MigrateToLatest<TConfiguration>(string connectionString)
            where TConfiguration : DbMigrationsConfiguration, new()
        {
            if (!MSSQLService.DatabaseExists(connectionString))
            {
                MSSQLService.DatabaseCreate(connectionString);
            }
            var configuration = new TConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient")
            };
            var dbMigrator = new DbMigrator(configuration);
            dbMigrator.Update();
        }
    }
}