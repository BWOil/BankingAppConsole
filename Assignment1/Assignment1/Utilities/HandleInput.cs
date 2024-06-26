﻿using System;
using Assignment1.Manager;
using Assignment1.Models;
using TextLibrary;
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
                NormalText.DisplayErrorMessage("Invalid input.\n");
            }
        }

        public static decimal HandleDecimalInput(string prompt, AccountManager accountManager, Account selectedAccount, TransactionType transactionType)
        {
            while (true)
            {
                Console.Write(prompt);

                if (decimal.TryParse(Console.ReadLine(), out var enteredAmount))
                {
                    if (IsValidAmount(enteredAmount, transactionType, selectedAccount, accountManager))
                    {
                        return enteredAmount;
                    }
                }
                else
                {
                    NormalText.DisplayErrorMessage("Invalid amount. Please enter a valid number.");
                }
            }
        }

        private static bool IsValidAmount(decimal amount, TransactionType transactionType, Account selectedAccount, AccountManager accountManager)
        {
            bool takeMoneyCondition = transactionType == TransactionType.Withdraw || transactionType == TransactionType.Transfer;

            if (amount > 0.01m && takeMoneyCondition)
            {
                decimal feeAmount = transactionType == TransactionType.Withdraw ? 0.05m : 0.1m;
                decimal balance = selectedAccount.Balance;

                if (!accountManager.AccountQualifiesForFreeServiceFee(selectedAccount) && amount + feeAmount > balance )
                {
                    NormalText.DisplayErrorMessage($"Insufficient funds because of service fee ${feeAmount:F2}");
                    return false;
                }
                else if (amount > balance)
                {
                    NormalText.DisplayErrorMessage("Insufficient funds.");
                    return false;
                    // checking account 
                } else if (!accountManager.AccountQualifiesForFreeServiceFee(selectedAccount) && selectedAccount.AccountType == "C" && amount + feeAmount > balance - 300)
                {
                    NormalText.DisplayErrorMessage($"Insufficient funds because the minimum balance of checking account is $300 and service fee charge ${feeAmount:F2}");
                    return false;
                } else if (selectedAccount.AccountType == "C" && amount > balance - 300)
                {
                    NormalText.DisplayErrorMessage($"Insufficient funds because the minimum balance of checking account is $300");
                    return false;
                }
            }
            else if (amount < 0.01m)
            {
                NormalText.DisplayErrorMessage("Amount must be at least $0.01.");
                return false;
            }

            return true;
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
                    NormalText.DisplayErrorMessage($"Input too long. Please enter a maximum of {maxLength} characters.\n");
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
                        NormalText.DisplayErrorMessage($"Cannot select the same account to transfer money\n");
                    else
                    {
                        var accountList = accountManager.GetAccountByAccountNumber(accountNumber);
                        if (accountList.Count() != 0)
                            return accountList[0]; // Input is within the max length
                        NormalText.DisplayErrorMessage($"Account number does not exist\n");
                    }            
                }
                else
                {
                    NormalText.DisplayErrorMessage($"Invalid account number input\n");
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
                        NormalText.DisplayErrorMessage("This is the last page.\n");
                } else if (input == "p")
                {
                    int testPage = currentPage - 1;
                    if (testPage > 0)
                        return input;
                    else
                        NormalText.DisplayErrorMessage("This is the first page.\n");
                } else if (input == "q")
                {
                    return input;
                }

                else
                {
                    NormalText.DisplayErrorMessage($"Invalid input. Please try again\n");
                }
            }
        }

    }
}