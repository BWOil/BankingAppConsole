using System;
using System.Data;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assignment1.Manager
{

	public class CustomerManager
	{
        private readonly string _connectionString;

        public CustomerManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        //check if any customer records exist
        public bool AnyExistingCustomer()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "select count(*) from Customer";

            var count = (int)command.ExecuteScalar();

            // Print the count to the console for verification
            //Console.WriteLine($"Number of customers in the database: {count}");

            return count > 0;
        }

        public List<Customer> GetCustomersAndAddress()
        {
            using var connection = new SqlConnection(_connectionString);



            using var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";

            var accountManager = new AccountManager(_connectionString);
            var loginManager = new LoginManager(_connectionString);


            return command.GetDataTable().Select().Select(x => new Customer
            {
                CustomerID = x.Field<int>("CustomerID"),
                Name = x.Field<string>("Name"),
                Address = x.Field<string>("Address"),
                City = x.Field<string>("City"),
                PostCode = x.Field<string>("PostCode"),
                Accounts = accountManager.GetAccounts(x.Field<int>("CustomerID")),
                Login = loginManager.GetLogin(x.Field<int>("CustomerID"))[0]


            }).ToList();
        }

        public void InsertCustomer(Customer customer)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into Customer (CustomerID, Name, Address, City, PostCode) values (@CustomerID, @Name, @Address, @City, @PostCode)";
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Name", customer.Name);
            command.Parameters.AddWithValue("Address", customer.Address.GetObjectOrDbNull());
            command.Parameters.AddWithValue("City", customer.City.GetObjectOrDbNull());
            command.Parameters.AddWithValue("PostCode", customer.PostCode.GetObjectOrDbNull());

            command.ExecuteNonQuery();
        }




    }
}
