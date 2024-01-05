using System;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
    public static class DatabaseManager
    {
        public static void CreateTables(string connectionString)
        {
            //if (string.IsNullOrEmpty(connectionString))
            //    throw new InvalidOperationException("The connection string cannot be null or empty.");

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("Sql/CreateTables.sql");

            command.ExecuteNonQuery();
        }
    }
}
