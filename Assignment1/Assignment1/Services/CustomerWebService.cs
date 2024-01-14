using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Assignment1.DTO;
using Assignment1.Manager;
using Assignment1.Models;

namespace Assignment1.Services
{
    public class CustomerWebService
    {
        public static void GetAndSaveCustomer(
            AccountManager accountManager,
            CustomerManager customerManager,
            LoginManager loginManager,
            TransactionManager transactionManager)
        {
            if (customerManager.AnyExistingCustomer())
                return;

            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";
            using var client = new HttpClient();
            var json = client.GetStringAsync(Url).Result;

            var customerDTOs = JsonConvert.DeserializeObject<List<DTO.CustomerDTO>>(json, new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            });

            foreach (var customerDTO in customerDTOs)
            {
                var customer = MapCustomerDTOToCustomer(customerDTO);
                customerManager.InsertCustomer(customer);

                if (customerDTO.Login != null)
                {
                    var login = MapLoginDTOToLogin(customerDTO.Login);
                    loginManager.InsertLogin(login, customer.CustomerID);
                }

                foreach (var accountDTO in customerDTO.Accounts)
                {
                    var account = MapAccountDTOToAccount(accountDTO);
                    accountManager.InsertAccount(account);

                    foreach (var transactionDTO in accountDTO.Transactions)
                    {
                        var transaction = MapTransactionDTOToTransaction(transactionDTO);
                        transactionManager.InsertTransaction(transaction);
                    }
                }
            }
        }

        private static Customer MapCustomerDTOToCustomer(DTO.CustomerDTO dto)
        {
            // Map from CustomerDTO to Customer
            return new Customer
            {
                CustomerID = dto.CustomerID,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                PostCode = dto.PostCode
            };
        }

        private static Login MapLoginDTOToLogin(DTO.LoginDTO dto)
        {
            // Map from LoginDTO to Login
            return new Login
            {
                LoginID = dto.LoginID,
                PasswordHash = dto.PasswordHash
            };
        }

        private static Account MapAccountDTOToAccount(DTO.AccountDTO dto)
        {
            // Map from AccountDTO to Account
            return new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountType = dto.AccountType,
                Balance = dto.Balance
            };
        }

        private static Transaction MapTransactionDTOToTransaction(DTO.TransactionDTO dto)
        {
            // Map from TransactionDTO to Transaction
            return new Transaction
            {
                TransactionType = dto.TransactionType,
                AccountNumber = dto.AccountNumber,
                Amount = dto.Amount,
                Comment = dto.Comment,
                TransactionTimeUtc = dto.TransactionTimeUtc
            };
        }
    }
}

