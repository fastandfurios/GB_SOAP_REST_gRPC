using System.Collections.Generic;

namespace LibraryService.Services.Interfaces
{
    public interface IRepository<T>
    {
        int Add(T item);

        int Update(T item);

        int Delete(T item);

        IList<T> GetAll();

        T GetById(string id);
    }
}
