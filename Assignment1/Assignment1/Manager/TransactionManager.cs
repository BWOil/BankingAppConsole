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

        public List<Transaction> GetTransactions ()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction]";

            return ReturnTransactionList(command);
        }

        public List<Transaction> GetTransactionsByAccountNumber(int accountNumber)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction] where AccountNumber = @accountNumber order by TransactionTimeUtc DESC";
            command.Parameters.AddWithValue("accountNumber", accountNumber);

            return ReturnTransactionList(command);

        }

        private List<Transaction> ReturnTransactionList(SqlCommand command)
        {
            return command.GetDataTable().Select().Select(x => new Transaction
            {
                TransactionID = x.Field<int>("TransactionID"),
                TransactionType = x.Field<string>("TransactionType"),
                AccountNumber = x.Field<int>("AccountNumber"),
                DestinationAccountNumber = x["DestinationAccountNumber"] == DBNull.Value ? null : (int)x["DestinationAccountNumber"],
                Amount = x.Field<decimal>("Amount"),
                Comment = x.Field<string>("Comment"),
                TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc")
            }).ToList();
        }

        public void InsertTransaction(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "insert into [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc) values (@TransactionType, @AccountNumber, @DestinationAccountNumber, @Amount, @Comment, @TransactionTimeUtc)";

            command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType); // Use the transaction type from the object
            command.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
            command.Parameters.AddWithValue("@DestinationAccountNumber", transaction.DestinationAccountNumber.GetObjectOrDbNull());
            command.Parameters.AddWithValue("@Amount", transaction.Amount);
            command.Parameters.AddWithValue("@Comment", transaction.Comment.GetObjectOrDbNull());
            command.Parameters.AddWithValue("@TransactionTimeUtc", transaction.TransactionTimeUtc);

            command.ExecuteNonQuery();
        }


    }
}
