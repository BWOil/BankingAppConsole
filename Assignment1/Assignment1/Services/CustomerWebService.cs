using System;
using Newtonsoft.Json;
using Assignment1.Models;
using Assignment1.Manager;

namespace Assignment1.Services
{


    public class CustomerWebService
	{
        //public static void GetAndSaveCustomer()
        //{
        //}

        //checked loading from json and deserialized obj.
        //private static void Main()
        //{
        //    ////Check if any people already exist and if they do stop.
        //    //if (customerManager.Any())
        //    //    return;

        //    const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        //    using var client = new HttpClient();
        //    var json = client.GetStringAsync(Url).Result;

        //    Console.WriteLine(json);

        //    // Convert JSON string to Customer objects.
        //    var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
        //    {
        //        // See here for DateTime format string documentation:
        //        // https://learn.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
        //        DateFormatString = "dd/MM/yyyy"
        //    });

        //    // ReSharper disable once PossibleNullReferenceException
        //    // Process objects, in this case just printing for demonstration purposes.
        //    foreach (var customer in customers)
        //    {
        //        Console.WriteLine($"Customer ID: {customer.CustomerID}, Name: {customer.Name}");
        //        Console.WriteLine($"Address: {customer.Address}, City: {customer.City}, PostCode: {customer.PostCode}");
        //        Console.WriteLine($"Accounts: ");

        //        var accounts = customer.Accounts;
        //        foreach (var account in accounts)
        //        {
        //            Console.WriteLine($"\tAccount Number: {account.AccountNumber}, Account Type: {account.AccountType}");

        //            var transactions = account.Transactions;
        //            foreach (var transaction in transactions)
        //            {
        //                Console.WriteLine($"\t\tAmount: {transaction.Amount}");
        //                Console.WriteLine($"\t\tComment: {transaction.Comment}");
        //                Console.WriteLine($"\t\tTransactionTimeUtc: {transaction.TransactionTimeUtc}");
        //                Console.WriteLine();
        //            }
        //        }

        //        var login = customer.Login;
        //        Console.WriteLine($"LoginID: {login.LoginID}, PasswordHash: {login.PasswordHash}");

        //    }

        //}



        //public static void GetAndSaveCustomer(
        //AccountManager accountManager, CustomerManager customerManager, LoginManager loginManager, TransactionManager transactionManager)
        //{
        //    // Check if any people already exist and if they do stop.
        //    if (customerManager.AnyExistingCustomer())
        //        return;

        //    const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        //    // Contact webservice.
        //    using var client = new HttpClient();
        //    var json = client.GetStringAsync(Url).Result;

        //    // Convert JSON into objects.
        //    var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
        //    {
        //        // See here for DateTime format string documentation:
        //        // https://learn.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
        //        DateFormatString = "dd/MM/yyyy"
        //    });

        //    // Insert into database.
        //    foreach (var customer in customers)
        //    {
        //        // Insert customer
        //        customerManager.InsertCustomer(customer);

        //        // Insert login
        //        customer.Login.CustomerID = customer.CustomerID;
        //        loginManager.InsertLogin(customer.Login, customer.CustomerID);

        //        //Insert Account
        //        foreach (var account in customer.Accounts)
        //        {
        //            account.CustomerID = customer.CustomerID;
        //            accountManager.InsertAccount(account);

        //            //Insert Transaction
        //            foreach (var transaction in account.Transactions)
        //            {
        //                transaction.AccountNumber = account.AccountNumber;
        //                transactionManager.InsertTransaction(transaction);
        //            }
        //        }



        //        }
        //    }


        public static void GetAndSaveCustomer(
    AccountManager accountManager, CustomerManager customerManager, LoginManager loginManager, TransactionManager transactionManager)
        {
            // Check if any people already exist and if they do stop.
            if (customerManager.AnyExistingCustomer())
                return;

            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

            // Contact webservice.
            using var client = new HttpClient();
            var json = client.GetStringAsync(Url).Result;

            // Convert JSON into objects.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            });

            // Insert into database.
            foreach (var customer in customers)
            {
                try
                {
                    Console.WriteLine($"1st CustomerID: {customer.CustomerID}"); // Print CustomerID
                    // Insert customer
                    customerManager.InsertCustomer(customer);

                    // Insert login
                    //customer.Login.CustomerID = customer.CustomerID;
                    loginManager.InsertLogin(customer.Login, customer.CustomerID);

                    // Insert Account
                    foreach (var account in customer.Accounts)
                    {
                        Console.WriteLine(account.AccountNumber);
                        Console.WriteLine($"CUStCustomerID: {customer.CustomerID}"); // Print CustomerID
                        Console.WriteLine($"Acc CustomerID: {account.CustomerID}"); // Print CustomerID
                        account.CustomerID = customer.CustomerID;
                        accountManager.InsertAccount(account);

                        // Insert Transaction
                        foreach (var transaction in account.Transactions)
                        {
                            transaction.AccountNumber = account.AccountNumber;
                            transactionManager.InsertTransaction(transaction);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or print the error message
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    // Optionally, log the stack trace or other details
                    // Logger.LogError(ex.ToString());
                }
            }
        }




    }



}

