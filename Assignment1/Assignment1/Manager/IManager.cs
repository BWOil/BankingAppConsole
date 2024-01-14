using System;
namespace Assignment1.Manager.ItemsManager
{
	public interface IManager<T>
	{
        List<T> GetAll();
        List<T> GetById(int id);
        void Insert(T entity);
    }
}

