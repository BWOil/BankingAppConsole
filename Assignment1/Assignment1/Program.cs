using Assignment1;
using Assignment1.Manager;

using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString(nameof(Assignment1));

//DatabaseManager.CreateTables(connectionString);

var facadeOperation = new FacadeOperation(connectionString);

facadeOperation.LoadingData();
facadeOperation.RunProgram();

