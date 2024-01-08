using System;
using SimpleHashing.Net;
namespace Assignment1
{
	public static class LoginSystem
	{
        private static string passwordHash =
"Rfc2898DeriveBytes$50000$" +
"MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE=";


		public static void LoginMenu()
		{
            var continueLogin = true;
            while (continueLogin)
            {
                Console.Write("Enter Login ID: ");
                var loginID = Console.ReadLine();

                Console.Write("Enter Password: ");
                var password = ReadPassword();
                Console.WriteLine();
                if (VerifyPassword(password))
                {
                    new Menu().Run();
                    continueLogin = false;
                }
                else
                {
                    Console.WriteLine("The password is incorrect. Please try again!");
                }
            }
        }

        private static bool VerifyPassword(string password)
        {
            return new SimpleHash().Verify(password, passwordHash);
        }

        private static string ReadPassword()
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

