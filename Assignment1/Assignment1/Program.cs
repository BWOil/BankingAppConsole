﻿using Assignment1;
using Assignment1.Manager;
using Assignment1.Services;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString(nameof(Assignment1));

//DatabaseManager.CreateTables(connectionString);

var accountManager = new AccountManager(connectionString);
var customerManager = new CustomerManager(connectionString);
var transactionManager = new TransactionManager(connectionString);
var loginManager = new LoginManager(connectionString);

CustomerWebService.GetAndSaveCustomer(accountManager, customerManager, loginManager, transactionManager);

var loginCustomer = new LoginSystem(customerManager).Run();
new Menu(loginCustomer, customerManager, transactionManager, accountManager).Run();

