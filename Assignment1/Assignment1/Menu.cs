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
                        Console.WriteLine("Transfer Money\n");
                        // Add transfer functionality here if needed
                        break;
                    case 4:
                        DisplayMyStatement();
                        break;
                    case 5:
                        Logout();
                        //menuOn = false;
                        break;
                    case 6:
                        ApplyTextColour.BlueText("exiting\n");
                        ApplyTextColour.BlueText("Good bye!\n");
                        Environment.Exit(0);
                        break;
                    default:
                        throw new UnreachableException();
                }
            }
        }

        private void ProcessTransaction(TransactionType transactionType)
        {
            var operation = transactionType.ToString();
            Console.WriteLine($"{operation} Money\n");

            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return;
            }

            DisplayAccountsWithIndex(accounts);
            int accountIndex = HandleInput.HandleSelection("Select an account: ", accounts.Count);
            var selectedAccount = accounts[accountIndex - 1];

            AccountUtilities.PrintAccountDetails(selectedAccount);
            decimal amount = HandleInput.HandleDecimalInput($"Enter {operation.ToLower()} amount (minimum $0.01): ",
                                                           "Invalid amount. Please enter a number greater than $0.01.");
            if (amount < 0.01m || (transactionType == TransactionType.Withdraw && amount > selectedAccount.Balance))
            {
                Console.WriteLine(transactionType == TransactionType.Withdraw ? "Insufficient funds." : "Invalid amount.");
                return;
            }

            string comment = HandleInput.HandleStringInput("Enter comment (max length 30): ", 30);
            AccountUtilities.PerformTransaction(_accountManager, selectedAccount, amount, comment, transactionType);
            Console.WriteLine($"{operation} of ${amount} successful. Account balance is ${selectedAccount.Balance}.");
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

        //private void DisplayMyStatement()
        //{
        //    //Console.WriteLine("My Statement\n");
        //    // ----------- add the account menu
        //    var allAccounts = _customer.Accounts;
        //    DisplayAccount("My Statement");

        //    int option = HandleInput.HandleSelection("Select an account: ", allAccounts.Count);
        //    var currentAccount = allAccounts[option - 1];
        //    DisplayAccountAndTransationList(currentAccount);



        //    //Console.WriteLine(currentAccountNumber);

        //    //Console.WriteLine(transactionList.Count);

        //}

        private void DisplayMyStatement()
        {
            Console.WriteLine("--- My Statement ---\n");

            DisplayAccountsWithIndex(_customer.Accounts);

            int option = HandleInput.HandleSelection("Select an account: ", _customer.Accounts.Count);
            var currentAccount = _customer.Accounts[option - 1];
            DisplayAccountAndTransationList(currentAccount); // Corrected method call
        }

        //private void DisplayAccountAndTransationList(Account account)
        //{
        //    string accountType = account.AccountType == "S" ? "Savings" : "Checking";
        //    Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
        //    const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,10:N2} | {4,-25} | {5}";
        //    var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);
        //    Console.WriteLine(Format, "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment");
        //    Console.WriteLine(new string('-', 120));
        //    foreach (var transaction in transactionList)
        //    {
        //        string transactionTypeDisplay = GetTransactionTypeDisplay(transaction.TransactionType); // Get the display value for transaction type
        //        Console.WriteLine(Format, transaction.TransactionID, transactionTypeDisplay, transaction.DestinationAccountNumber == null ? "N/A" : transaction.DestinationAccountNumber, transaction.Amount, transaction.TransactionTimeUtc, transaction.Comment);
        //    }
        //    Console.WriteLine();
        //}

        private string GetColoredAmount(decimal amount, string transactionType)
        {
            string formattedAmount = $"{amount:C2}"; // Include $ in the formatted amount

            if (transactionType == "D")
            {
                // Green color for deposit
                return $"\u001b[32m{formattedAmount}\u001b[0m"; // \u001b[32m sets green color, \u001b[0m resets the color
            }
            else
            {
                // Red color for withdraw or service charge
                return $"\u001b[31m{formattedAmount}\u001b[0m"; // \u001b[31m sets red color, \u001b[0m resets the color
            }
        }

        private void DisplayAccountAndTransationList(Account account)
        {
            string accountType = account.AccountType == "S" ? "Savings" : "Checking";
            Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-20} | {4,-25} | {5,-25}";
            var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);

            // Improved column headers with proper alignment
            Console.WriteLine(Format, "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment");
            Console.WriteLine(new string('-', 120));

            foreach (var transaction in transactionList)
            {
                string transactionTypeDisplay = GetTransactionTypeDisplay(transaction.TransactionType); // Get the display value for transaction type

                // Format the amount with 2 decimal places and apply color
                string amountFormatted = GetColoredAmount(transaction.Amount, transaction.TransactionType);

                // Improved row format with proper alignment
                string rowFormat = "{0,-5} | {1,-20} | {2,-20} | {3,-20} | {4,-25} | {5,-25}";

                Console.WriteLine(rowFormat, transaction.TransactionID, transactionTypeDisplay,
                    transaction.DestinationAccountNumber == null ? "N/A" : transaction.DestinationAccountNumber,
                    amountFormatted, transaction.TransactionTimeUtc.ToString("M/d/yyyy h:mm:ss tt"),
                    transaction.Comment);
            }
            Console.WriteLine();
        }






        private string GetTransactionTypeDisplay(string transactionType)
        {
            switch (transactionType)
            {
                case "D":
                    return "Deposit";
                case "W":
                    return "Withdraw";
                case "T":
                    return "Transfer";
                case "S":
                    return "ServiceCharge";
                default:
                    return "Unknown";
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
            Console.WriteLine("Are you sure you want to log out? (Y/N)");
            string input = HandleInput.HandleStringInput("", 1); // Using HandleStringInput for a single character

            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Logging out...");

                // Start the login process again
                var loginCustomer = new LoginSystem(_customerManager).Run();
                if (loginCustomer != null)
                {
                    new Menu(loginCustomer, _customerManager, _transactionManager, _accountManager).Run();
                }
                else
                {
                    Console.WriteLine("Exiting application.");
                    Environment.Exit(0); // Exits the application
                }
            }
            else
            {
                Console.WriteLine("Logout cancelled. Returning to main menu.");
            }
        }
    }
}

