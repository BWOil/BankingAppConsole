using System;
namespace Assignment1.Models
{
	public class Transaction
	{
		public int TransactionID { get; set; }
		public string TransactionType { get; set; }
		public int AccountNumber { get; set; }
		public int? DestinationAccountNumber { get; set; }
		public decimal Amount { get; set; }
		public string Comment { get; set; }
		public DateTime TransactionTimeUtc { get; set; }
	}
}
