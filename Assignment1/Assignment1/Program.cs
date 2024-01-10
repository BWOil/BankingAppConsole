using Assignment1;
using Assignment1.Manager;
using Assignment1.Services;
using Assignment1.Utilities;
using Microsoft.Extensions.Configuration;




var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var connectionString = configuration.GetConnectionString(nameof(Assignment1));
// OR
//var connectionString = configuration.GetConnectionString("WebServiceAndDatabaseExample");

//DatabaseManager.CreateTables(connectionString);

var accountManager = new AccountManager(connectionString);
var customerManager = new CustomerManager(connectionString);
var transactionManager = new TransactionManager(connectionString);
var loginManager = new LoginManager(connectionString);

CustomerWebService.GetAndSaveCustomer(accountManager, customerManager, loginManager, transactionManager);


LoginSystem.LoginMenu();
//CustomerWebService.GetAndSavePeople(personManager, petManager, addressManager);
