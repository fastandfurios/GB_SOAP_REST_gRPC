using ConsoleClient.Application.Interfaces.Commands;
using ConsoleClient.Application.Services;
using LibraryWebService;

namespace ConsoleClient.Application.Commands
{
    public class UpdateCommand : ICommand<int>
    {
        public int Execute(object parameter)
        {
            if (parameter is not Book book) return 0;

            var libraryWebServiceSoapClient = WebService.GetService();

            libraryWebServiceSoapClient.Update(new Book
            {
                Id = book.Id,
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
