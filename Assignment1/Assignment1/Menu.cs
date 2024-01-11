using System;
using System.Diagnostics;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;

namespace Assignment1
{
	public class Menu
	{
        private readonly Customer _customer;
        private CustomerManager _customerManager;
        private readonly AccountManager _accountManager;

        public Menu(Customer customer, CustomerManager customerManager, AccountManager accountManager)
		{
            _customer = customer;
            _customerManager = customerManager;
            _accountManager = accountManager;
        }

        public void Run()
        {
            var menuOn = true;

            while(menuOn)
            {
                PrintMenu();
                Console.Write("Enter an option: ");
                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out var option) || !option.IsInRange(1, 6))
                {
                    ApplyTextColour.RedText("Invalid input.\n");
                    continue;
                }

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
                        Console.WriteLine("My Statement\n");
                        break;
                    case 5:
                        Logout();
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


        private void Deposit()
        {
            var accounts = _accountManager.GetAccounts(_customer.CustomerID);
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts available.");
                return;
            }

            DisplayAccountsWithIndex(accounts);
            Console.Write("Select an account to deposit by number: ");
            if (!int.TryParse(Console.ReadLine(), out var accountIndex) || accountIndex < 1 || accountIndex > accounts.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedAccount = accounts[accountIndex - 1];
            // Display selected account details
            Console.WriteLine($"Selected Account: \n Account Number: {selectedAccount.AccountNumber} \n " +
                $"Account Type: {selectedAccount.AccountType} \n Current Balance: ${selectedAccount.Balance}\n" +
                $" Available Balance: ${selectedAccount.Balance}\n");

            Console.Write("Enter deposit amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            Console.Write("Enter comment (n to quit, max length 30): ");
            var comment = Console.ReadLine();
            if (comment.Length > 30)
            {
                ApplyTextColour.RedText("Comment exceeded maximum length!\n");
                return;
            }

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
            Console.Write("Select an account to withdraw from by number: ");
            if (!int.TryParse(Console.ReadLine(), out var accountIndex) || accountIndex < 1 || accountIndex > accounts.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var selectedAccount = accounts[accountIndex - 1];
            // Display selected account details
            Console.WriteLine($"Selected Account: \n Account Number: {selectedAccount.AccountNumber} \n " +
                $"Account Type: {selectedAccount.AccountType} \n Current Balance: ${selectedAccount.Balance}");

            Console.Write("Enter withdrawal amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (amount > selectedAccount.Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            Console.Write("Enter comment (max length 30): ");
            var comment = Console.ReadLine();
            if (comment.Length > 30)
            {
                Console.WriteLine("Comment too long.");
                return;
            }

            _accountManager.Withdraw(selectedAccount, amount, comment);
            Console.WriteLine($"Withdrawal of ${amount} successful. New balance is ${selectedAccount.Balance}.");

            DisplayAccountsWithIndex(accounts);
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
                Console.WriteLine(Format, index, account.AccountType, account.AccountNumber, account.Balance);
                index++;
            }
            Console.WriteLine();
        }

       
        public void Logout()
        {
            //needs to be fixed

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

