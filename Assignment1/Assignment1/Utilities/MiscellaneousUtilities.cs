using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Assignment1.Utilities
{

	public static class MiscellaneousUtilities
	{
        public static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;

        public static DataTable GetDataTable(this SqlCommand command)
        {
            using var adapter = new SqlDataAdapter(command);

            var table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        public static object GetObjectOrDbNull(this object value) => value ?? DBNull.Value;
    }
}

