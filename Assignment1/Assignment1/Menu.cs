using System;
using Assignment1.Manager;
using Assignment1.Models;

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

