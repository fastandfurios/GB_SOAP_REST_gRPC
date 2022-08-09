using LibraryWebService;

namespace ConsoleClient.Application.Services
{
    public static class WebService
    {
        private static LibraryWebServiceSoapClient _service;

        public static LibraryWebServiceSoapClient GetService()
        {
            return _service ?? new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration.LibraryWebServiceSoap12);
        }
    }
}
