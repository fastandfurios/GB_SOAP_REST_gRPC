using System.Collections.Generic;
using LibraryService.Models;

namespace LibraryService.Services.Interfaces
{
    public interface ILibraryRepositoryService : IRepository<Book>
    {
        IList<Book> GetByTitle(string title);

        IList<Book> GetByAuthor(string authorName);

        IList<Book> GetByCategory(string category);
    }
}
