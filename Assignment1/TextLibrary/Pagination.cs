using System;
namespace TextLibrary
{
	public static class Pagination
	{
		public static void Bottom(int currentPage, int totalPage)
		{
            Console.WriteLine($"Page {currentPage} of {totalPage}\n\nOptions: {(currentPage == totalPage ? "" : "n (next page) | ")}{(currentPage == 1 ? "" : "p (previous page) | ")}q (quit)");
        }
	}
}

