using System;
using System.Data;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
    public class AccountManager
    {

        private readonly string _connectionString;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public List<Account> GetAccounts(int customerID)
        //{
        //    using var connection = new SqlConnection(_connectionString);
            

        //    using var command = connection.CreateCommand();
        //    command.CommandText = "select * from Account where CustomerID = @customerID";
        //    command.Parameters.AddWithValue("customerID", customerID);
        //    // OR
        //    // command.Parameters.AddWithValue(nameof(personID), personID);

        //    var transactionManager = new TransactionManager(_connectionString);

        //    return command.GetDataTable().Select().Select(x => new Account
        //    {
        //        AccountNumber = x.Field<int>("AccountNumber"),
        //        AccountType = x.Field<string>("AccountType"),
        //        CustomerID = customerID,
        //        Balance = x.Field<decimal>("Balance"),
        //        Transactions = transactionManager.GetTransactions(x.Field<int>("AccountNumber"))

        //    }).ToList();
        //}

        public void InsertAccount(Account account)
        {
            var balance = CalculateBalance(account);

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into Account (AccountNumber, AccountType, CustomerID, Balance) values (@AccountNumber, @AccountType, @CustomerID, @Balance)";
            command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("AccountType", account.AccountType);
            command.Parameters.AddWithValue("CustomerID", account.CustomerID);
            command.Parameters.AddWithValue("Balance", balance);

            command.ExecuteNonQuery();
        }

        private decimal CalculateBalance(Account account)
        {
            decimal balance = 0;
            var transactions = account.Transactions;
            foreach( var transaction in transactions)
            {
                balance += transaction.Amount;
            }
            return balance;


        }

        public void Deposit(Account account, decimal amount)
        {
            
            //Console.WriteLine("--Deposit--");

            //var balance = account.Balance;

            //balance += amount;

            //CalculateBalance();




            //update db as well
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //using var command = connection.CreateCommand();
            //command.CommandText =
            //    "update Inventory set Price = @price where InventoryID = @inventoryID";
            //command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            //command.Parameters.AddWithValue("AccountType", account.AccountType);
            //command.Parameters.AddWithValue("CustomerID", account.CustomerID);
            //command.Parameters.AddWithValue("Balance", balance);

            //command.ExecuteNonQuery();
        }

        public void Withdraw(Account account)
        {
            //update db as well
        }

        public List<Account> GetAccounts(int customerID)
        {
            var accounts = new List<Account>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("@customerID", customerID);

            var transactionManager = new TransactionManager(_connectionString);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var account = new Account
                {
                    AccountNumber = reader.GetInt32(reader.GetOrdinal("AccountNumber")),
                    AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                    CustomerID = customerID,
                    Balance = reader.GetDecimal(reader.GetOrdinal("Balance")),
                    // Populate Transactions if required and if transactionManager.GetTransactions is implemented
                    // Transactions = transactionManager.GetTransactions(reader.GetInt32(reader.GetOrdinal("AccountNumber")))
                };
                accounts.Add(account);
            }

            return accounts;
        }




    }
}

