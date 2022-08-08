using ConsoleClient.Application.Interfaces.Commands;
using ConsoleClient.Application.Models;
using ConsoleClient.Application.Services;

namespace ConsoleClient.Application.Commands
{
    public class GetBooksByTitleCommand : ICommand<List<BookClient>>
    {
        public List<BookClient> Execute(object parameter)
        {
            var title = parameter as string;
            var libraryWebServiceSoapClient = WebService.GetService();
            var books = libraryWebServiceSoapClient.GetBooksByTitle(title);

            return books.Select(item => new BookClient
                {
                    AgeLimit = item.AgeLimit,
                    PublicationDate = item.PublicationDate,
                    Category = item.Category,
                    Lang = item.Lang,
                    Pages = item.Pages,
                    Title = item.Title,
                    Authors = item.Authors.Select(author => new AuthorClient
                        {
                            Name = author.Name,
                            Lang = author.Lang
                        })
                        .ToArray()
                })
                .ToList();
        }
    }
}
