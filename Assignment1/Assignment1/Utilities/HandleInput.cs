using System;

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

        public static decimal HandleDecimalInput(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out var result))
                {
                    if (result >= 0.01m)
                    {
                        return result; // Valid input, return the result
                    }
                    else
                    {
                        ApplyTextColour.RedText("Amount must be at least $0.01.\n"); // Invalid input, show specific error
                    }
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

    }
}