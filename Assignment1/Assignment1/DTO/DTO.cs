using System;
using System.Collections.Generic;

namespace Assignment1.DTO
{
	
    public class AccountDTO
    {
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
    }

    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public List<AccountDTO> Accounts { get; set; }
        public LoginDTO Login { get; set; }
    }

    public class LoginDTO
    {
        public string LoginID { get; set; }
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; }
    }

    public class TransactionDTO
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

