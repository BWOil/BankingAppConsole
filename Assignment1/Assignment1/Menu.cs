﻿using System;
using System.Diagnostics;
using System.Linq;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;
using TextLibrary;
using static Assignment1.Menu;

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
                        ProcessTransaction(TransactionType.Transfer);
                        break;
                    case 4:
                        DisplayMyStatement();
                        break;
                    case 5:
                        Logout();
                        break;
                    case 6:
                        exitProgram();
                        
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
            Account destinationAccount = null;

            AccountUtilities.PrintAccountDetails(selectedAccount);

            if (transactionType == TransactionType.Transfer)
            {
                destinationAccount = HandleInput.HandleAccountNumberInput("Enter destination account number: ", _accountManager, selectedAccount.AccountNumber);
                AccountUtilities.PrintAccountDetails(destinationAccount);
            }

            decimal amount = HandleInput.HandleDecimalInput($"Enter {operation.ToLower()} amount (minimum $0.01): ", _accountManager, selectedAccount, transactionType);

            string comment = HandleInput.HandleStringInput("Enter comment (n to quit, max length 30): ", 30);

            // Check if the customer typed "n" to quit
            if (comment.ToLower() == "n")
            {
                Console.WriteLine("Transaction cancelled. Returning to the main menu.\n");
                return;
            }

            try
            {
                AccountUtilities.PerformTransaction(_accountManager, selectedAccount, amount, comment, transactionType, destinationAccount);

                // Calculate available balance after performing the transaction
                decimal availableBalance = selectedAccount.Balance - (selectedAccount.AccountType == "C" ? 300 : 0);

                Console.WriteLine($"{operation} of ${amount} successful. Account balance is ${selectedAccount.Balance:F2}, Available Balance: ${availableBalance:F2}.\n");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}\n");
            }
        }


        private void PrintMenu()
        {
            NormalText.TitleWithContent(
                _customer.Name,
                "[1] Deposit\n[2] Withdraw\n[3] Transfer\n[4] My Statement\n[5] Logout\n[6] Exit\n");

        }

        private void DisplayMyStatement()
        {

            var selectAccount = DisplayAccountsWithIndex("My Statement");

            DisplayAccountAndTransactionList(selectAccount);

            Console.WriteLine("");

        }

        private void DisplayAccountAndTransactionList(Account account)
        {
            string accountType = account.AccountType == "S" ? "Savings" : "Checking";
            decimal availableBalance = account.Balance - (account.AccountType == "C" ? 300 : 0);

            Console.WriteLine($"{accountType} {account.AccountNumber}, Account Balance: ${account.Balance:F2}, Available Balance: ${availableBalance:F2}\n");

            var transactionList = _transactionManager.GetTransactionsByAccountNumber(account.AccountNumber);

            int page = 1;
            while (true)
            {
                DisplayTransactionsPage(transactionList, page);

                int totalPage = (int)Math.Ceiling(transactionList.Count / 4.0);

                Pagination.Bottom(page, totalPage);

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

        private void DisplayTransactionsPage(List<Transaction> transactionList, int page)
        {
            Console.WriteLine();
      
            int space = 25;

            //// Display the header on each page
            Table.Record(new List<string> { "ID", "Transaction Type", "Destination", "Amount", "Time", "Comment" }, space);
   
            Table.Divider(130);

            int startIndex = (page - 1) * 4;
            int endIndex = Math.Min(startIndex + 3, transactionList.Count - 1);

            for (int i = startIndex; i <= endIndex; i++)
            {
                var transaction = transactionList[i];
                string transactionTypeDisplay = GetTransactionTypeDisplay(transaction.TransactionType);
                string amountFormatted = GetColoredAmount(transaction.Amount, transaction.TransactionType);

                string destination = transaction.TransactionType == "D" || transaction.TransactionType == "W" || transaction.TransactionType == "S"
                ? "N/A" : transaction.DestinationAccountNumber.ToString();

                Table.Record(new List<string> { transaction.TransactionID.ToString(), transactionTypeDisplay,
                    destination.ToString(), amountFormatted,
                    transaction.TransactionTimeUtc.ToString("M/d/yyyy h:mm:ss tt"), transaction.Comment }, space);
            }
        }


        private string GetColoredAmount(decimal amount, string transactionType)
        {
            string formattedAmount = $"{amount:C2}";

            if (transactionType == "D")
                return $"\u001b[32m{formattedAmount}\u001b[0m"; // Green color for deposit
            else
                return $"\u001b[31m{formattedAmount}\u001b[0m"; // Red color for withdraw or service charge
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

        private Account DisplayAccountsWithIndex(string title)
        {
            NormalText.MenuTitle(title);
            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            int space = 20;
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return null;
            }

            Table.Record(new List<string> { "No", "Account Type", "Account Number", "Balance" }, space);
            Table.Divider(75);

            int index = 1;
            foreach (var account in accounts)
            {
                string accountType = account.AccountType == "S" ? "Saving" : (account.AccountType == "C" ? "Checking" : "Unknown");

                Table.Record(new List<string> { index.ToString(), accountType, account.AccountNumber.ToString(), account.Balance.ToString("F2") }, space);
                index++;
            }
            Console.WriteLine();
            // select an account
            int accountIndex = HandleInput.HandleSelection("Select an account: ", accounts.Count);
            return accounts[accountIndex - 1];


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

        private void exitProgram()
        {
            ApplyTextColour.BlueText("Program Ending.\n");
            Environment.Exit(0);
        }
    }
}
