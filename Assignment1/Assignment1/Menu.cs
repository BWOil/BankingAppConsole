using System;
namespace Assignment1
{
	public class Menu
	{
		public Menu()
		{

		}

        public void Run()
        {
            PrintMenu();
        }

        private void PrintMenu()
        {
            Console.WriteLine(
                """
            --- <Username> ---
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

