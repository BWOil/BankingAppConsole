using System;
using SimpleHashing.Net;
using Assignment1.Manager;
using Assignment1.Models;
using Assignment1.Utilities;
using System.Security;
using System.Runtime.InteropServices;

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
                        break;
                    }                              
                }
                if (!foundCustomer)
                    ApplyTextColour.RedText("\nInvalid Login ID. Please try again!\n");

            }

            return null;
        }

        private bool VerifyPassword(Customer customer)
        {
            var passwordHash = customer.Login.PasswordHash;
            var inputPassword = true;
            while (inputPassword)
            {
                Console.Write("Enter Password: ");
                using (var password = ReadPassword())
                {
                    if (new SimpleHash().Verify(ConvertToString(password), passwordHash))
                    {
                        return true;
                    }
                    ApplyTextColour.RedText("Invalid password. Please try again!\n");
                }
            }
            
            return false;
        }

        private SecureString ReadPassword()
        {
            // Instantiate the secure string.
            SecureString password = new SecureString();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true); // intercept the key
                if (key.Key != ConsoleKey.Enter)
                {
                    // Append the character to the password.
                    password.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            password.MakeReadOnly();

            Console.WriteLine(); // Move to the next line after the password has been entered
            return password;
        }

        private static string ConvertToString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }


    }
}
