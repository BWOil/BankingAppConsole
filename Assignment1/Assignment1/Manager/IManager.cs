using System;
using Microsoft.Data.SqlClient;
namespace Assignment1.Manager
{
	public interface IManager<T>
	{
        public List<T> GetAll();
        public void Insert(T entity);
    }
}

