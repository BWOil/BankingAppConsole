using System;
using System.Data;
using System.Transactions;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
    public class AccountManager: IManager<Account>
    {

        private readonly string _connectionString;
        private readonly TransactionManager _transactionManager;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
            _transactionManager = new TransactionManager(connectionString);
        }

        public List<Account> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);


            using var command = connection.CreateCommand();
            command.CommandText = "select * from Account";

            return ReturnList(command);

        }

        public List<Account> GetAccounts(int customerID)
        {
            using var connection = new SqlConnection(_connectionString);


            using var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @customerID";
            command.Parameters.AddWithValue("customerID", customerID);

            return ReturnList(command);

        }

        public List<Account> GetAccountByAccountNumber(int accountNumber)
        {
            using var connection = new SqlConnection(_connectionString);


            using var command = connection.CreateCommand();
            command.CommandText = "select * from Account where AccountNumber = @accountNumber";
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            return ReturnList(command);
        }

        private List<Account> ReturnList(SqlCommand command)
        {
            var transactionManager = new TransactionManager(_connectionString);

            return command.GetDataTable().Select().Select(x => new Account
            {
                AccountNumber = x.Field<int>("AccountNumber"),
                AccountType = x.Field<string>("AccountType"),
                CustomerID = x.Field<int>("CustomerID"),
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

        public void Insert(Account account)
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
            CreateTransaction(account, amount, "D", comment, null); // "D" for Deposit
        }

        public void Withdraw(Account account, decimal amount, string comment)
        {

            CreateTransaction(account, amount, "W", comment, null); // "W" for Withdraw, amount is negative

            if (!AccountQualifiesForFreeServiceFee(account))
            {
                CreateTransaction(account, 0.05m, "S", "", null);
            }
        }


        public void Transfer(Account account, decimal amount, string comment, Account destinationAccount)
        {
            CreateTransaction(account, amount, "T", comment, destinationAccount);
            destinationAccount.Balance += amount;
            UpdateAccount(destinationAccount);
            if (!AccountQualifiesForFreeServiceFee(account))
            {
                CreateTransaction(account, 0.1m , "S", "", null);
            }
        }

        private void CreateTransaction(Account account, decimal amount, string transactionType, string comment, Account destinationAccount)
        {
            var transaction = new Models.Transaction
            {
                AccountNumber = account.AccountNumber,
                Amount = Math.Abs(amount), // Ensure the amount is positive
                DestinationAccountNumber = destinationAccount != null ? destinationAccount.AccountNumber : null,
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
            else if (transactionType == "W" || transactionType == "T" || transactionType == "S") // Withdraw or Transfer
            {
                account.Balance -= amount; // Subtract the amount for withdrawal
                
            }
            else
            {
                throw new InvalidOperationException("Invalid transaction type.");
            }
            UpdateAccount(account);
            _transactionManager.Insert(transaction);
        }

        public void UpdateAccount(Account account)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                "update Account set Balance = @Balance where AccountNumber = @AccountNumber";
            command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("Balance", account.Balance);
            command.ExecuteNonQuery();

        }
    }
}
