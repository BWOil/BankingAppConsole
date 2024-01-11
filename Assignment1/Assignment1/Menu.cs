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

        public Menu(Customer customer, CustomerManager customerManager)
		{
            _customer = customer;
            _customerManager = customerManager;
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
    }
}

