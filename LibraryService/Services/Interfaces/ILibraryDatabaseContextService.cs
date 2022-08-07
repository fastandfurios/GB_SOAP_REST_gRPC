using System.Collections.Generic;
using LibraryService.Models;

namespace LibraryService.Services.Interfaces
{
    public interface ILibraryDatabaseContextService
    {
        IList<Book> Books { get; }
    }
}
