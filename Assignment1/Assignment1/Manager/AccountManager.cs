using System;
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

        public bool AccountQualifiesForFreeServiceFee(Account account)
        {
            int count = 0;
            foreach(var transaction in account.Transactions)
            {
                string transactionType = transaction.TransactionType;
                if (transactionType == "T" || transactionType == "W")
                    count++;
            }
            return count < 2;
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
            foreach (var transaction in transactions)
            {
                balance += transaction.Amount;
            }
            return balance;
        }

        public void Deposit(Account account, decimal amount, string comment)
        {
            CreateTransaction(account, amount, "D", comment); // "D" for Deposit
        }

        public void Withdraw(Account account, decimal amount, string comment)
        {
            if (account.Balance - amount < (account.AccountType == "S" ? 0 : 300))
            {
                throw new InvalidOperationException("Insufficient funds for withdrawal.");
            }

            CreateTransaction(account, -amount, "W", comment); // "W" for Withdraw, amount is negative
        }

        private void CreateTransaction(Account account, decimal amount, string transactionType, string comment)
        {
            var transaction = new Models.Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = Math.Abs(amount), // Ensure the amount is positive
                Comment = comment,
                TransactionType = transactionType,
                TransactionTimeUtc = DateTime.UtcNow
            };

            account.Transactions.Add(transaction);

            // Adjust account balance based on transaction type
            if (transactionType == "D") // Deposit
            {
                account.Balance += amount; // Add the amount for deposit
            }
            else if (transactionType == "W") // Withdraw
            {
                account.Balance -= amount; // Subtract the amount for withdrawal
            }
            else
            {
                throw new InvalidOperationException("Invalid transaction type.");
            }

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
           
        }
    }
}
