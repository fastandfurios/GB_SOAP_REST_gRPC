using ConsoleClient.Application.Interfaces.Commands;
using LibraryWebService;
using ConsoleClient.Application.Services;

namespace ConsoleClient.Application.Commands
{
    public class AddCommand : ICommand<int>
    {
        public int Execute(object parameter)
        {
            if (parameter is not Book book) return 0;

            var libraryWebServiceSoapClient = WebService.GetService();

            libraryWebServiceSoapClient.Add(new Book
            {
                AgeLimit = book.AgeLimit,
                Authors = book.Authors,
                Category = book.Category,
                Lang = book.Lang,
                Pages = book.Pages,
                PublicationDate = book.PublicationDate,
                Title = book.Title
            });

            return 1;
        }
    }
}
