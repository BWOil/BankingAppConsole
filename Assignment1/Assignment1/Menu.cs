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
        //private readonly AccountManager _accountManager;

        public Menu(Customer customer, CustomerManager customerManager, TransactionManager transactionManager
            //AccountManager accountManager
            )
		{
            _customer = customer;
            _customerManager = customerManager;
            _transactionManager = transactionManager;
            //_accountManager = accountManager;

        }

        public void Run()
        {
            var menuOn = true;

            while(menuOn)
            {
                PrintMenu();
                int option = handleSelection("Enter an option: ", 6);

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Deposit Money\n");
                        DisplayAccount("Account");
                        Console.Write("Select an account: ");
                        var depositInput = Console.ReadLine();
                        Console.WriteLine();
                        // show selected deposit account


                        // add amount
                        Console.Write("Enter amount: ");
                        var depositamount = Console.ReadLine();

                        // add comment
                        Console.Write("Enter comment (n to quit, mac length 30): ");
                        var comment = Console.ReadLine();

                        //print
                        Console.WriteLine(
                            """Withdraw of $ {depositamount} successful, account balance is now {balance}"""
                                );
                        break;
                    case 2:
                        Console.WriteLine("Withdraw Money\n");
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

            int option = handleSelection("Select an account: ", allAccounts.Count);
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



        private int handleSelection(string question, int length)
        {
            bool optionInvalid = true;
            while (optionInvalid)
            {
                Console.Write(question);
                var input = Console.ReadLine();
                Console.WriteLine();

                if (int.TryParse(input, out var option) && option.IsInRange(1, length))
                {
                    return option;                   
                }
                ApplyTextColour.RedText("Invalid input.\n");
            }
            return 0;
            
        }
              
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

    }
}

