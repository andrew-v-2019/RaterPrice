using System;
using System.Data.SqlClient;

namespace Migrator
{
    public static class MSSQLService
    {
        public static bool TableExists(string connectionString)
        {
            var masterConnStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var dbName = masterConnStringBuilder["Initial Catalog"];
            masterConnStringBuilder["Initial Catalog"] = "master";

            using (var masterConnection = new SqlConnection(masterConnStringBuilder.ConnectionString))
            {
                var sqlCommand = new SqlCommand
                {
                    CommandText =
                        string.Format(@"SELECT count(1) from sys.sysdatabases where name = '{0}'",
                            dbName),
                    Connection = masterConnection
                };

                masterConnection.Open();
                var result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                masterConnection.Close();

                return result > 0;
            }
        }

        public static bool DatabaseExists(string connectionString)
        {
            var masterConnStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var dbName = masterConnStringBuilder["Initial Catalog"];
            masterConnStringBuilder["Initial Catalog"] = "master";

            using (var masterConnection = new SqlConnection(masterConnStringBuilder.ConnectionString))
            {
                var sqlCommand = new SqlCommand
                {
                    CommandText =
                        string.Format(@"SELECT count(1) from sys.sysdatabases where name = '{0}'",
                            dbName),
                    Connection = masterConnection
                };

                masterConnection.Open();
                var result = Convert.ToInt32(sqlCommand.ExecuteScalar());
                masterConnection.Close();

                return result > 0;
            }
        }

        public static void DatabaseDelete(string connectionString)
        {
            var masterConnStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var dbName = masterConnStringBuilder["Initial Catalog"];
            masterConnStringBuilder["Initial Catalog"] = "master";

            using (var masterConnection = new SqlConnection(masterConnStringBuilder.ConnectionString))
            {
                var dropCommand = new SqlCommand
                {
                    CommandText = string.Format(@"ALTER DATABASE {0} 
                                                                        SET SINGLE_USER 
                                                                        WITH ROLLBACK IMMEDIATE;
                                                                        DROP DATABASE {0}", dbName),
                    Connection = masterConnection
                };

                masterConnection.Open();
                dropCommand.ExecuteNonQuery();
                masterConnection.Close();
            }
        }

        public static void DatabaseCreate(string connectionString)
        {
            var masterConnStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var dbName = masterConnStringBuilder["Initial Catalog"];
            masterConnStringBuilder["Initial Catalog"] = "master";

            using (var masterConnection = new SqlConnection(masterConnStringBuilder.ConnectionString))
            {
                var createCommand = new SqlCommand
                {
                    CommandText =
                        string.Format("CREATE DATABASE {0} COLLATE Cyrillic_General_CI_AS",
                            dbName),
                    Connection = masterConnection
                };

                masterConnection.Open();
                createCommand.ExecuteNonQuery();
                masterConnection.Close();
            }
        }
    }
}