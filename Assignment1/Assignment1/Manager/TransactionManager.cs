using System;
using System.Data;
using Assignment1.Models;
using Assignment1.Utilities;
using Microsoft.Data.SqlClient;

namespace Assignment1.Manager
{
	public class TransactionManager
	{
        private readonly string _connectionString;

        public TransactionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Transaction> GetTransactions (int accountNumber)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction]";

            return command.GetDataTable().Select().Select(x => new Transaction
            {
                TransactionID = x.Field<int>("TransactionID"),
                TransactionType = x.Field<string>("TransactionType"),
                AccountNumber = accountNumber,
                DestinationAccountNumber = x.Field<int>("DestinationAccountNumber"),
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc")
            }).ToList();
        }

        public void InsertTransaction(Transaction transaction)
        {
            var transactionType = "D";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into [Transaction] (TransactionType, AccountNumber, Amount, Comment, TransactionTimeUtc) values (@TransactionType,@AccountNumber, @Amount, @Comment, @TransactionTimeUtc)";


            command.Parameters.AddWithValue("TransactionType", transactionType);
            command.Parameters.AddWithValue("AccountNumber", transaction.AccountNumber);
            command.Parameters.AddWithValue("Amount", transaction.Amount);
            command.Parameters.AddWithValue("Comment", transaction.Comment.GetObjectOrDbNull());
            command.Parameters.AddWithValue("TransactionTimeUtc", transaction.TransactionTimeUtc);

            command.ExecuteNonQuery();
        }

    }
}
