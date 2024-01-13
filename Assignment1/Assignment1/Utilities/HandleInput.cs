using System;
using Assignment1.Manager;
using Assignment1.Models;
using static Assignment1.Menu;

namespace Assignment1.Utilities
{
    public static class HandleInput
    {
        public static int HandleSelection(string question, int length)
        {
            while (true)
            {
                Console.Write(question);
                var input = Console.ReadLine();
                Console.WriteLine();

                if (int.TryParse(input, out var option) && option >= 1 && option <= length)
                {
                    return option;
                }
                ApplyTextColour.RedText("Invalid input.\n");
            }
        }

        public static decimal HandleDecimalInput(string prompt, string errorMessage, Account selectedAccount, TransactionType transactionType)
        {
            bool takeMoneyCondition = transactionType == TransactionType.Withdraw || transactionType == TransactionType.Transfer;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out var result))
                {
                    if (takeMoneyCondition && result > selectedAccount.Balance)
                        ApplyTextColour.RedText(takeMoneyCondition ? "Insufficient funds.\n" : "Invalid amount.\n");
                    else if (result >= 0.01m)
                        return result; // Valid input, return the result
                    else
                        ApplyTextColour.RedText("Amount must be at least $0.01.\n"); // Invalid input, show specific error
                }
                else
                {
                    ApplyTextColour.RedText(errorMessage + "\n"); // Parsing error, show general error
                }
            }
        }


        public static string HandleStringInput(string prompt, int maxLength)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input.Length <= maxLength)
                {
                    return input; // Input is within the max length
                }
                else
                {
                    ApplyTextColour.RedText($"Input too long. Please enter a maximum of {maxLength} characters.\n");
                }
            }
        }

        public static Account HandleAccountNumberInput(string prompt, AccountManager accountManager, int currentAccountNumber)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input.Length == 4 && int.TryParse(input, out var accountNumber))
                {
                    if (accountNumber == currentAccountNumber)
                        ApplyTextColour.RedText($"Cannot select the same account to transfer money\n");
                    else
                    {
                        var accountList = accountManager.GetAccountByAccountNumber(accountNumber);
                        if (accountList.Count() != 0)
                            return accountList[0]; // Input is within the max length
                        ApplyTextColour.RedText($"Account number does not exist\n");
                    }
                    
                }
                else
                {
                    ApplyTextColour.RedText($"Invalid account number input\n");
                }
            }
        }


        public static string HandlePaginationInput(string prompt, int totalPages, int currentPage)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();


                if (input == "n")
                {
                    int testPage = currentPage + 1;
                    if (testPage <= totalPages)
                        return input;
                    else
                        ApplyTextColour.RedText($"This is the last page.\n");
                } else if (input == "p")
                {
                    int testPage = currentPage - 1;
                    if (testPage > 0)
                        return input;
                    else
                        ApplyTextColour.RedText($"This is the first page.\n");
                } else if (input == "q")
                {
                    return input;
                }

                else
                {
                    ApplyTextColour.RedText($"Invalid input. Please try again\n");
                }
            }
        }

    }
}