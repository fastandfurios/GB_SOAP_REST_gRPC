using LibraryService.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LibraryServiceReference;

namespace LibraryService.Web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;
        private readonly LibraryWebServiceSoapClient _libraryWebServiceSoapClient;

        public LibraryController(ILogger<LibraryController> logger, LibraryWebServiceSoapClient libraryWebServiceSoapClient)
        {
            _logger = logger;
            _libraryWebServiceSoapClient = libraryWebServiceSoapClient;
        }

        public IActionResult Index(SearchType searchType, string searchString)
        {
            var bookCategoryViewModel = new BookCategoryViewModel
            {
                Books = new Book[] {}
            };

            if (!string.IsNullOrEmpty(searchString) && searchString.Length > 3)
            {
                bookCategoryViewModel.Books = searchType switch
                {
                    SearchType.Title => _libraryWebServiceSoapClient.GetBooksByTitle(searchString),
                    SearchType.Category => _libraryWebServiceSoapClient.GetBooksByCategory(searchString),
                    SearchType.Author => _libraryWebServiceSoapClient.GetBooksByAuthor(searchString),
                    _ => bookCategoryViewModel.Books
                };
            }

            return View(bookCategoryViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}