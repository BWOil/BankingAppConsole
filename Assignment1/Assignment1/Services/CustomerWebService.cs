﻿using System;
using Newtonsoft.Json;
using Assignment1.Models;
using Assignment1.Manager;
using System.Threading.Tasks;

namespace Assignment1.Services
{
    public class CustomerWebService
    {
        public static async Task GetAndSaveCustomer(
            AccountManager accountManager, CustomerManager customerManager, LoginManager loginManager, TransactionManager transactionManager)
        {
            // Check if any people already exist and if they do, stop.
            if (customerManager.AnyExistingCustomer())
                return;

            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

            // Contact webservice.
            using var client = new HttpClient();
            var json = await client.GetStringAsync(Url);

            // Convert JSON into objects.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            });

            // Insert into database.
            foreach (var customer in customers)
            {
                // Insert customer
                customerManager.Insert(customer);

                // Insert login
                customer.Login.CustomerID = customer.CustomerID;
                loginManager.Insert(customer.Login);

                // Insert Account
                foreach (var account in customer.Accounts)
                {
                    account.CustomerID = customer.CustomerID;
                    accountManager.Insert(account);

                    // Insert Transaction
                    foreach (var transaction in account.Transactions)
                    {
                        transaction.AccountNumber = account.AccountNumber;
                        transactionManager.Insert(transaction);
                    }
                }
            }
        }
    }
}
