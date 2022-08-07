using System.Collections.Generic;
using System.Text;
using LibraryService.Models;
using LibraryService.Services.Interfaces;
using Newtonsoft.Json;

namespace LibraryService.Services.Implementation
{
    public class LibraryDatabaseContext : ILibraryDatabaseContextService

    {
    private IList<Book> _libraryDatabase;

    public IList<Book> Books => _libraryDatabase;

    public LibraryDatabaseContext()
    {
        Initialize();
    }

    private void Initialize()
    {

        _libraryDatabase =
            JsonConvert.DeserializeObject<List<Book>>(Encoding.UTF8.GetString(Properties.Resources.books));
    }
    }
}
