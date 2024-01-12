using System;
using System.Diagnostics;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;

namespace Assignment1
{
	public class Menu
	{
        private readonly Customer _customer;  // to be edit for read only
        private CustomerManager _customerManager;
        private TransactionManager _transactionManager;
        private readonly AccountManager _accountManager;

        public Menu(Customer customer, CustomerManager customerManager, TransactionManager transactionManager,
            AccountManager accountManager
            )
		{
            _customer = customer;
            _customerManager = customerManager;
            _transactionManager = transactionManager;
            _accountManager = accountManager;

        }

        public void Run()
        {
            var menuOn = true;

            while(menuOn)
            {
                PrintMenu();
                int option = HandleInput.HandleSelection("Enter an option: ", 6);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Deposit Money\n");
                        Deposit();

                        break;
                    case 2:
                        Console.WriteLine("Withdraw Money\n");
                        Withdraw();
                        break;
                    case 3:
                        Console.WriteLine("Transfer Money\n");
                        break;
                    case 4:
                        DisplayMyStatement();
                        break;
                    case 5:
                        menuOn = false;
                        break;
                    case 6:
                        Console.WriteLine("exit\n");
                        ApplyTextColour.BlueText("Good bye!\n");

                        break;
                    default:
                        throw new UnreachableException();

                }
            }
            
        }

        private void PrintMenu()
        {
            Console.WriteLine(
            $"""
            --- {_customer.Name} ---
            [1] Deposit
            [2] Withdraw
            [3] Transfer
            [4] My Statement
            [5] Logout
            [6] Exit

            """);
        }

        private void DisplayMyStatement()
        {
            //Console.WriteLine("My Statement\n");
            // ----------- add the account menu
            var allAccounts = _customer.Accounts;
            DisplayAccount("My Statement");

            int option = HandleInput.HandleSelection("Select an account: ", allAccounts.Count);
            var currentAccount = allAccounts[option - 1];
            DisplayAccountAndTransationList(currentAccount);



            //Console.WriteLine(currentAccountNumber);
            
            //Console.WriteLine(transactionList.Count);

        }

        private void DisplayAccountAndTransationList(Account account)
        {
            string accountType = account.AccountType == "S" ? "Savings" : "Checking";
            Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3, -20} | {4, -25} | {5}";
            var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);
            Console.WriteLine(Format, "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment");
            Console.WriteLine(new string('-', 120));
            foreach (var transaction in transactionList)
            {
                
                Console.WriteLine(Format, transaction.TransactionID, transaction.TransactionType, transaction.DestinationAccountNumber == null ? transaction.DestinationAccountNumber : "N/A", transaction.Amount, transaction.TransactionTimeUtc, transaction.Comment);
            }
            Console.WriteLine();
        }

        //private void UpdateCurrentCustomer()
        //{
        //    _customer = _customerManager.GetCustromerByID(_customer.CustomerID);
        //}



        private void DisplayAccount(string title)
        {
            // Assuming Format is "No | Account Type | Account Number | Balance"
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-10}";

            Console.WriteLine($"--- {title} ---\n");
            Console.WriteLine(Format, "No", "Account Type", "Account Number", "Balance");
            Console.WriteLine(new string('-', 60));

            // Assuming _accountManager.GetAccounts() returns a list of accounts
            //var accounts = _accountManager.GetAccounts(_customer.CustomerID); // or however you obtain the customer's accounts
            var accounts = _customer.Accounts;
            int accountNo = 1;
            foreach (var account in accounts)
            {
                Console.WriteLine(Format, accountNo, account.AccountType, account.AccountNumber, account.Balance);
                accountNo++;
            }
            Console.WriteLine();
        }


        private void Deposit()
        {
            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return;
            }
            DisplayAccountsWithIndex(accounts);

            int accountIndex = HandleInput.HandleSelection("Select an account to deposit by number: ", accounts.Count);
            var selectedAccount = accounts[accountIndex - 1];

            PrintSelectedAccountDetails(selectedAccount);

            decimal amount = HandleInput.HandleDecimalInput("Enter deposit amount (minimum $0.01): ", "Invalid amount. Please enter a number greater than $0.01.");
            if (amount < 0.01m)
            {
                return;
            }

            string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);

            _accountManager.Deposit(selectedAccount, amount, comment);
            Console.WriteLine($"Deposit of ${amount} successful. New balance is ${selectedAccount.Balance}.");

            DisplayAccountsWithIndex(accounts);
        }

        private void Withdraw()
        {
            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return;
            }

            DisplayAccountsWithIndex(accounts);
            int accountIndex = HandleInput.HandleSelection("Select an account to withdraw from by number: ", accounts.Count);
            var selectedAccount = accounts[accountIndex - 1];

            PrintSelectedAccountDetails(selectedAccount);

            decimal amount = HandleInput.HandleDecimalInput("Enter withdrawal amount (minimum $0.01): ", "Invalid amount. Please enter a number greater than $0.01.");
            if (amount < 0.01m || amount > selectedAccount.Balance)
            {
                Console.WriteLine(amount > selectedAccount.Balance ? "Insufficient funds." : "Invalid amount.");
                return;
            }

            string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);

            _accountManager.Withdraw(selectedAccount, amount, comment);
            Console.WriteLine($"Withdrawal of ${amount} successful. New balance is ${selectedAccount.Balance}.");

            DisplayAccountsWithIndex(accounts);
        }



        private void PrintSelectedAccountDetails(Account selectedAccount)
        {
            // Display selected account details
            Console.WriteLine($"Selected Account: \n Account Number: {selectedAccount.AccountNumber} \n " +
                              $"Account Type: {selectedAccount.AccountType} \n Current Balance: ${selectedAccount.Balance}" +
                              "Available Balance: ${selectedAccount.Balance} \n");
        }



        private void DisplayAccountsWithIndex(List<Account> accounts)
        {
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-10}";
            Console.WriteLine("--- Accounts ---");
            Console.WriteLine(Format, "No", "Account Type", "Account Number", "Balance");
            Console.WriteLine(new string('-', 60));

            int index = 1;
            foreach (var account in accounts)
            {
                string accountType = account.AccountType == "S" ? "Saving" : (account.AccountType == "C" ? "Checking" : "Unknown");
                Console.WriteLine(Format, index, accountType, account.AccountNumber, account.Balance.ToString("F2"));
                index++;
            }
            Console.WriteLine();
        }


        public void Logout()
        {
            ////needs to be fixed
            //Console.WriteLine("Logging out...");
            //// You can add any additional cleanup or session-ending logic here if needed
            //// Redirect back to the login screen
            //var loginCustomer = new LoginSystem(_customerManager).Run();
            //if (loginCustomer != null)
            //{
            //    new Menu(loginCustomer, _customerManager, _accountManager).Run();
            //}
            //else
            //{
            //    Console.WriteLine("Exiting application.");
            //}
        }
    }
}

