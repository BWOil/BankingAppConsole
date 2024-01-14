using System;
using System.Data;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
	public class LoginManager: IManager<Login>
	{
		private readonly string _connectionString;

		public LoginManager(string connectionString)
		{
			_connectionString  = connectionString;
		}

		public List<Login> GetAll()
		{
			using var connection = new SqlConnection(_connectionString);
        

            using var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            return ReturnList(command);
        }   

        public List<Login> GetLoginByCustomerID(int customerID)
        {
            using var connection = new SqlConnection(_connectionString);


            using var command = connection.CreateCommand();
            command.CommandText = "select * from Login where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            return ReturnList(command);
        }

        private List<Login> ReturnList(SqlCommand command)
        {
            return command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = x.Field<string>("LoginID"),
                CustomerID = x.Field<int>("CustomerID"),
                PasswordHash = x.Field<string>("PasswordHash")

            }).ToList();
        }

        public void Insert(Login login)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into Login (LoginID, CustomerID, PasswordHash) values (@LoginID, @CustomerID, @PasswordHash)";
            command.Parameters.AddWithValue("LoginID", login.LoginID);
            command.Parameters.AddWithValue("CustomerID", login.CustomerID);
            command.Parameters.AddWithValue("PasswordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }
	}
}

