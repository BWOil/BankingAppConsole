using Assignment1.Manager;
using Assignment1.Services;

namespace Assignment1
{
    public class FacadeOperation
	{
        //private readonly SubsystemA a = new SubsystemA();
        //private readonly SubsystemB b = new SubsystemB();
        //private readonly SubsystemC c = new SubsystemC();
        private AccountManager accountManager;
        private CustomerManager customerManager;
        private TransactionManager transactionManager;
        private LoginManager loginManager;

        public FacadeOperation(string connectionString)
        {
            accountManager = new AccountManager(connectionString);
            customerManager = new CustomerManager(connectionString);
            transactionManager = new TransactionManager(connectionString);
            loginManager = new LoginManager(connectionString);
        }

        public void LoadingData()
        {
            CustomerWebService.GetAndSaveCustomer(accountManager, customerManager, loginManager, transactionManager);
        }

        public void RunProgram()
        {
            var loginCustomer = new LoginSystem(customerManager).Run();
            new Menu(loginCustomer, customerManager, transactionManager, accountManager).Run();
        }

        
	}
}

