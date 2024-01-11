﻿using System;
using System.Data;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
	public class LoginManager
	{
		private readonly string _connectionString;

		public LoginManager(string connectionString)
		{
			_connectionString = connectionString;
		}

		public List<Login> GetLogin(int customerID)
		{
			using var connection = new SqlConnection(_connectionString);
        

            using var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            return command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = x.Field<string>("LoginID"),
                CustomerID = customerID,
                PasswordHash = x.Field<string>("PasswordHash")
               
            }).ToList();
        }

        public void InsertLogin(Login login, int customerID)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into Login (LoginID, CustomerID, PasswordHash) values (@LoginID, @CustomerID, @PasswordHash)";
            command.Parameters.AddWithValue("LoginID", login.LoginID);
            command.Parameters.AddWithValue("CustomerID", customerID);
            command.Parameters.AddWithValue("PasswordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }
	}
}

