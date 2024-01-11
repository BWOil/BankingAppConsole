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
        private TransactionManager _transactionManager;

        public Menu(Customer customer, CustomerManager customerManager, TransactionManager transactionManager)
		{
            _customer = customer;
            _customerManager = customerManager;
            _transactionManager = transactionManager;
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
            Console.WriteLine("My Statement\n");
            var allAccounts = _customer.Accounts;
            int option = handleSelection("Select an account: ", 2);
            var currentAccount = allAccounts[option];
            Console.WriteLine(currentAccount.AccountNumber);
            var transactionList = _transactionManager.GetTransactions(currentAccount.AccountNumber);
            Console.WriteLine(transactionList.Count);

        }



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
    }
}

