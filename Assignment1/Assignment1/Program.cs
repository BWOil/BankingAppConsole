using Assignment1;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var connectionString = configuration.GetConnectionString(nameof(Assignment1));


        //DatabaseManager.CreateTables(connectionString); // Uncomment if needed


        var facadeOperation = new FacadeOperation(connectionString);

        // Await the asynchronous loading of data
        await facadeOperation.LoadingDataAsync();

        // Proceed to run the program
        facadeOperation.RunProgram();
    }
}
