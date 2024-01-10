using System;
using SimpleHashing.Net;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;

namespace Assignment1
{
	public class LoginSystem
	{

        private readonly CustomerManager _customerManager;

        public LoginSystem(CustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

		public Customer Run()
		{
            var continueLogin = true;
            var customerList = _customerManager.GetCustomersAndAddress();

            while (continueLogin)
            {
                Console.Write("Enter Login ID: ");
                var loginID = Console.ReadLine();


                // search for target customer
                bool foundCustomer = false;
                foreach (var customer in customerList)
                {
                    if (customer.Login.LoginID == loginID)
                    {
                        foundCustomer = true;
                        if (VerifyPassword(customer))
                        {
                            ApplyTextColour.GreenText("\n\nLogin successfully!\n");
                            return customer;
                        }
                        else
                        {
                            ApplyTextColour.RedText("\n\nInvalid password. Please try again!\n");
                        }

                        break;
                    }
                    
                        

                }
                if (!foundCustomer)
                    ApplyTextColour.RedText("\nInvalid Login ID. Please try again!\n");




                //Console.WriteLine();
                //if (VerifyPassword(customer ))
                //{
                //    new Menu().Run();
                //    continueLogin = false;
                //}
                //else
                //{
                //    Console.WriteLine("The password is incorrect. Please try again!");
                //}
            }

            return null;
        }

        private bool VerifyPassword(Customer customer)
        {
            var passwordHash = customer.Login.PasswordHash;
            Console.Write("Enter Password: ");
            var password = ReadPassword();
            return new SimpleHash().Verify(password, passwordHash);
        }

        private string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // intercept the key
                if (key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to the next line after the password has been entered
            return password;
        }


    }
}
