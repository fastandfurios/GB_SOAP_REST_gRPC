using ConsoleClient.Application.Interfaces.Commands;
using ConsoleClient.Application.Services;
using LibraryWebService;

namespace ConsoleClient.Application.Commands
{
    public class GetByIdCommand : ICommand<Book>
    {
        public Book Execute(object parameter)
        {
            var id = parameter as string;
            var libraryWebServiceSoapClient = WebService.GetService();
            return libraryWebServiceSoapClient.GetById(id);
        }
    }
}
