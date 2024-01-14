using System;
namespace TextLibrary
{
	public static class Table
	{
		public static void Divider(int length)
		{
            Console.WriteLine(new string('-', length));
        }

		public static void Record(List<string> data)
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (i == 0)
					Console.Write($"{data[i],-5} | ");
				else if (i == data.Count - 1)
                    Console.Write($"{data[i]}");
                else
					Console.Write($"{data[i],-25} | ");
            }
			Console.WriteLine();

		}
	}
}

