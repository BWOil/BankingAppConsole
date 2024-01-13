using System;
using System.Diagnostics;
using System.Linq;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;

namespace Assignment1
{
    public class Menu
    {
        private readonly Customer _customer;
        private readonly CustomerManager _customerManager;
        private readonly TransactionManager _transactionManager;
        private readonly AccountManager _accountManager;

        public Menu(Customer customer, CustomerManager customerManager, TransactionManager transactionManager, AccountManager accountManager)
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
                        break;
                    case 6:
                        ApplyTextColour.BlueText("Exiting\n");
                        ApplyTextColour.BlueText("Goodbye!\n");
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
                $"--- {_customer.Name} ---\n" +
                "[1] Deposit\n" +
                "[2] Withdraw\n" +
                "[3] Transfer\n" +
                "[4] My Statement\n" +
                "[5] Logout\n" +
                "[6] Exit\n"
            );
        }


        private void DisplayMyStatement()
        {
            Console.WriteLine("--- My Statement ---\n");
            DisplayAccountsWithIndex(_customer.Accounts);

            int option = HandleInput.HandleSelection("Select an account: ", _customer.Accounts.Count);
            var currentAccount = _customer.Accounts[option - 1];
            DisplayAccountAndTransactionList(currentAccount);
        }

        private void DisplayAccountAndTransactionList(Account account)
        {
            string accountType = account.AccountType == "S" ? "Savings" : "Checking";
            Console.WriteLine($"{accountType} {account.AccountNumber}, Balance: ${account.Balance:F2}, Available Balance: ${account.Balance:F2}\n");
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-20} | {4,-25} | {5,-25}";
            var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);

            Console.WriteLine(Format, "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment");
            Console.WriteLine(new string('-', 120));

            foreach (var transaction in transactionList)
            {
                string transactionTypeDisplay = GetTransactionTypeDisplay(transaction.TransactionType);
                string amountFormatted = GetColoredAmount(transaction.Amount, transaction.TransactionType);
                string rowFormat = "{0,-5} | {1,-20} | {2,-20} | {3,-20} | {4,-25} | {5,-25}";

                Console.WriteLine(rowFormat, transaction.TransactionID, transactionTypeDisplay,
                    transaction.DestinationAccountNumber == null ? "N/A" : transaction.DestinationAccountNumber,
                    amountFormatted, transaction.TransactionTimeUtc.ToString("M/d/yyyy h:mm:ss tt"),
                    transaction.Comment);
            }
        }

        private string GetColoredAmount(decimal amount, string transactionType)
        {
            string formattedAmount = $"{amount:C2}";

            if (transactionType == "D")
            {
                return $"\u001b[32m{formattedAmount}\u001b[0m"; // Green color for deposit
            }
            else
            {
                return $"\u001b[31m{formattedAmount}\u001b[0m"; // Red color for withdraw or service charge
            }
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
            string input = HandleInput.HandleStringInput("", 1);

            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Logging out...");
                var loginCustomer = new LoginSystem(_customerManager).Run();
                if (loginCustomer != null)
                {
                    new Menu(loginCustomer, _customerManager, _transactionManager, _accountManager).Run();
                }
                else
                {
                    Console.WriteLine("Exiting application.");
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Logout cancelled. Returning to the main menu.");
            }
        }
    }
}
