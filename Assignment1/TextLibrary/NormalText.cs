using System;
namespace TextLibrary
{
	public static class NormalText
	{
		public static void MenuTitle(string title)
		{
			Console.WriteLine($"--- {title} ---\n");
		}

		public static void TitleWithContent(string title, string content)
		{
            NormalText.MenuTitle(title);
			Console.WriteLine(content);
			
		}

        public static void DisplayErrorMessage(string message)
        {
            ApplyTextColour.RedText($"{message}\n");
        }


    }
}

