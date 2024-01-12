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
                if (decimal.TryParse(Console.ReadLine(), out var result) && result > 0)
                {
                    return result;
                }
                ApplyTextColour.RedText(errorMessage + "\n");
            }
        }

        public static string HandleStringInput(string prompt, int maxLength)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            return input.Length <= maxLength ? input : input.Substring(0, maxLength);
        }
    }
}
