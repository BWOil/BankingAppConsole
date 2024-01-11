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
                        DisplayAccount();
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
                        Console.WriteLine("My Statement\n");
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

        private void DisplayAccount()
        {
            // Assuming Format is "No | Account Type | Account Number | Balance"
            const string Format = "{0,-5} | {1,-20} | {2,-20} | {3,-10}";

            Console.WriteLine("--- Accounts ---");
            Console.WriteLine(Format, "No", "Account Type", "Account Number", "Balance");
            Console.WriteLine(new string('-', 60));

            // Assuming _accountManager.GetAccounts() returns a list of accounts
            var accounts = _accountManager.GetAccounts(_customer.CustomerID); // or however you obtain the customer's accounts

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

