﻿using System;
using System.Data;
using System.Transactions;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
    public class AccountManager
    {

        private readonly string _connectionString;
        private readonly TransactionManager _transactionManager;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
            _transactionManager = new TransactionManager(connectionString);
        }

        public List<Account> GetAccounts(int customerID)
        {
            using var connection = new SqlConnection(_connectionString);


            using var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            var transactionManager = new TransactionManager(_connectionString);

            return command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = x.Field<int>("AccountNumber"),
                AccountType = x.Field<string>("AccountType"),
                CustomerID = customerID,
                Balance = x.Field<decimal>("Balance"),
                Transactions = transactionManager.GetTransactionsByAccountNumber(x.Field<int>("AccountNumber"))

            }).ToList();
        }

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

        public void Deposit(Account account, decimal amount, string comment)
        {
            var transaction = new Models.Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = amount,
                Comment = comment,
                TransactionType = "D", // "D" for Deposit
                TransactionTimeUtc = DateTime.UtcNow
            };
            account.Transactions.Add(transaction);
            account.Balance += amount;
            UpdateAccount(account);
            _transactionManager.InsertTransaction(transaction); // Correct call to insert transaction
        }

        public void Withdraw(Account account, decimal amount, string comment)
        {
            // Check for sufficient balance
            if (account.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }

            var transaction = new Models.Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = amount, // Keep it positive as per the database constraint
                Comment = comment,
                TransactionType = "W", // "W" for Withdraw
                TransactionTimeUtc = DateTime.UtcNow
            };

            account.Transactions.Add(transaction);
            account.Balance -= amount; // Subtract from balance
            UpdateAccount(account);
            _transactionManager.InsertTransaction(transaction);
        }


        public void UpdateAccount(Account account)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                "update Account set Balance = @Balance where AccountNumber = @AccountNumber";
            Console.WriteLine(account.AccountNumber);
            Console.WriteLine(account.Balance);
            command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("Balance", account.Balance);
            command.ExecuteNonQuery();
            try
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Rows affected: {rowsAffected}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating account: {ex.Message}");
            }
        }
    }
}
