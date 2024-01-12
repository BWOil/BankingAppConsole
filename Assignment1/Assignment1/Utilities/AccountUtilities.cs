using Assignment1.Manager;
using Assignment1.Models;

namespace Assignment1.Utilities
{
    public static class AccountUtilities
    {
        public static void PrintAccountDetails(Account account)
        {
            string accountType = account.AccountType == "S" ? "Saving" : (account.AccountType == "C" ? "Checking" : "Unknown");
            Console.WriteLine($"Account Number: {account.AccountNumber}\nAccount Type: {accountType}\nBalance: ${account.Balance:F2}\n");
        }

        public static void PerformTransaction(AccountManager accountManager, Account account, decimal amount, string comment, bool isDeposit)
        {
            if (isDeposit)
            {
                accountManager.Deposit(account, amount, comment);
            }
            else
            {
                if (amount > account.Balance)
                {
                    throw new InvalidOperationException("Insufficient funds for withdrawal.");
                }
                accountManager.Withdraw(account, amount, comment);
            }
        }
    }
}
