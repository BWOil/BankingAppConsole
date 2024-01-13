using System;
using System.Diagnostics;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;
using static Assignment1.Menu;

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

        public enum TransactionType
        {
            Deposit,
            Withdraw,
            Transfer,
            ServiceCharge
        }


        public void Run()
        {
            var menuOn = true;
            while (menuOn)
            {
                PrintMenu();
                int option = HandleInput.HandleSelection("Enter an option: ", 6);
                switch (option)
                {
                    case 1:
                        ProcessTransaction(TransactionType.Deposit);
                        break;
                    case 2:
                        ProcessTransaction(TransactionType.Withdraw);
                        break;
                    case 3:
                        ProcessTransaction(TransactionType.Transfer);
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

        private void ProcessTransaction(TransactionType transactionType)
        {
            var operation = transactionType.ToString();

            var selectedAccount = DisplayAccountsWithIndex(operation);
            

            AccountUtilities.PrintAccountDetails(selectedAccount);

            if (transactionType == TransactionType.Transfer)
            {
                Account destinationAccountNumber = HandleInput.HandleAccountNumberInput("Enter destination account number: ", _accountManager);
                AccountUtilities.PrintAccountDetails(destinationAccountNumber);
                
            }

            decimal amount = HandleInput.HandleDecimalInput($"Enter {operation.ToLower()} amount (minimum $0.01): ",
                                                           "Invalid amount. Please enter a number greater than $0.01.");
            bool takeMoneyCondition = transactionType == TransactionType.Withdraw || transactionType == TransactionType.Transfer;
            if (amount < 0.01m || (takeMoneyCondition && amount > selectedAccount.Balance))
            {
                Console.WriteLine(takeMoneyCondition ? "Insufficient funds." : "Invalid amount.");
                return;
            }

            string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);
            AccountUtilities.PerformTransaction(_accountManager, selectedAccount, amount, comment, transactionType);
            Console.WriteLine($"{operation} of ${amount} successful. New balance is ${selectedAccount.Balance}.");
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

            var selectAccount = DisplayAccountsWithIndex("My Statement");

            
            DisplayAccountAndTransationList(selectAccount);

            Console.WriteLine("");

        }

        private void DisplayAccountAndTransationList(Account account)
        {
            string accountType = account.AccountType == "S" ? "Savings" : "Checking";
            bool paginationOn = true;
            int page = 1;
            const string Format = "{0,-5} | {1,-18} | {2,-15} | {3, -15} | {4, -20} | {5}";
            while (paginationOn)
            {
                Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
                               
                var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);
                Console.WriteLine(Format, "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment");
                Console.WriteLine(new string('-', 120));
                int listLength = transactionList.Count();
                int totalPage = (int)Math.Ceiling(listLength / 4.0);
                
                int pageStart = page == 1 ? 0 : (page - 1) * 4;
                int pageEnd = pageStart + 3  < listLength - 1 ? pageStart + 3 : listLength - 1;

                for (int i = pageStart; i <= pageEnd; i++)
                {
                    Console.WriteLine(Format, transactionList[i].TransactionID, transactionList[i].TransactionType, transactionList[i].DestinationAccountNumber == null ? transactionList[i].DestinationAccountNumber : "N/A", transactionList[i].Amount, transactionList[i].TransactionTimeUtc, transactionList[i].Comment);
                }
                Console.WriteLine($"Page {page} of {totalPage}\n\nOptions: {(page == totalPage ? "" : "n (next page) | ")}{(page == 1 ? "" : "p (previous page) | ")}q (quit)");

                string option = HandleInput.HandlePaginationInput("Enter an option: ", totalPage, page);
                switch (option)
                {
                    case "n":
                        page++;
                        break;
                    case "p":
                        page--;
                        break;
                    case "q":
                        return;
                    default:
                        throw new UnreachableException();
                }
            }
        }

        //private void UpdateCurrentCustomer()
        //{
        //    _customer = _customerManager.GetCustromerByID(_customer.CustomerID);
        //}



        //private void DisplayAccount(string title)
        //{
        //    // Assuming Format is "No | Account Type | Account Number | Balance"
        //    const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-10}";

        //    Console.WriteLine($"--- {title} ---\n");
        //    Console.WriteLine(Format, "No", "Account Type", "Account Number", "Balance");
        //    Console.WriteLine(new string('-', 60));

        //    // Assuming _accountManager.GetAccounts() returns a list of accounts
        //    //var accounts = _accountManager.GetAccounts(_customer.CustomerID); // or however you obtain the customer's accounts
        //    var accounts = _customer.Accounts;
        //    int accountNo = 1;
        //    foreach (var account in accounts)
        //    {
        //        Console.WriteLine(Format, accountNo, account.AccountType, account.AccountNumber, account.Balance);
        //        accountNo++;
        //    }
        //    Console.WriteLine();
        //}

        public static string GetTransactionCode(TransactionType transactionType)
        {
            return transactionType switch
            {
                TransactionType.Deposit => "D",
                TransactionType.Withdraw => "W",
                TransactionType.Transfer => "T",
                TransactionType.ServiceCharge => "S",
                _ => throw new ArgumentOutOfRangeException(nameof(transactionType), transactionType, null)
            };
        }


        //private void Deposit()
        //{
        //    var accounts = _accountManager.GetAccounts(_customer.CustomerID);
        //    if (accounts.Count == 0)
        //    {
        //        Console.WriteLine("No accounts available.");
        //        return;
        //    }
        //    DisplayAccountsWithIndex(accounts);

        //    int accountIndex = HandleInput.HandleSelection("Select an account to deposit by number: ", accounts.Count);
        //    var selectedAccount = accounts[accountIndex - 1];

        //    PrintSelectedAccountDetails(selectedAccount);

        //    decimal amount = HandleInput.HandleDecimalInput("Enter deposit amount (minimum $0.01): ", "Invalid amount. Please enter a number greater than $0.01.");
        //    if (amount < 0.01m)
        //    {
        //        return;
        //    }

        //    string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);

        //    _accountManager.Deposit(selectedAccount, amount, comment);
        //    Console.WriteLine($"Deposit of ${amount} successful. New balance is ${selectedAccount.Balance}.");

        //    DisplayAccountsWithIndex(accounts);
        //}

        //private void Withdraw()
        //{
        //    var accounts = _accountManager.GetAccounts(_customer.CustomerID);
        //    if (accounts.Count == 0)
        //    {
        //        Console.WriteLine("No accounts available.");
        //        return;
        //    }

        //    DisplayAccountsWithIndex(accounts);
        //    int accountIndex = HandleInput.HandleSelection("Select an account to withdraw from by number: ", accounts.Count);
        //    var selectedAccount = accounts[accountIndex - 1];

        //    PrintSelectedAccountDetails(selectedAccount);

        //    decimal amount = HandleInput.HandleDecimalInput("Enter withdrawal amount (minimum $0.01): ", "Invalid amount. Please enter a number greater than $0.01.");
        //    if (amount < 0.01m || amount > selectedAccount.Balance)
        //    {
        //        Console.WriteLine(amount > selectedAccount.Balance ? "Insufficient funds." : "Invalid amount.");
        //        return;
        //    }

        //    string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);

        //    _accountManager.Withdraw(selectedAccount, amount, comment);
        //    Console.WriteLine($"Withdrawal of ${amount} successful. New balance is ${selectedAccount.Balance}.");

        //    DisplayAccountsWithIndex(accounts);
        //}



        //private void PrintSelectedAccountDetails(Account account)
        //{
        //    // Display selected account details
        //    //Console.WriteLine($"Account Number: {selectedAccount.AccountNumber} \n " +
        //    //                  $"Account Type: {selectedAccount.AccountType} \n Current Balance: ${selectedAccount.Balance}" +
        //    //                  "Available Balance: ${selectedAccount.Balance} \n");
        //    string accountType = account.AccountType == "S" ? "Savings" : "Checking";
        //    Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
        //}



        private Account DisplayAccountsWithIndex(string title)
        {
            Console.WriteLine($"--- {title} ---\n");

            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return null;
            }

            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-10}";

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
            // select an account
            int accountIndex = HandleInput.HandleSelection("Select an account: ", accounts.Count);
            return accounts[accountIndex - 1];

            
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

