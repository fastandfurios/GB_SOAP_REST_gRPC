using LibraryServiceReference;

namespace LibraryService.Web.Extensions
{
    public static class LibraryServiceEx
    {
        public static IServiceCollection AddLibraryWebServiceSoapClient(this IServiceCollection services)
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(LibraryWebServiceSoapClient),
                new LibraryWebServiceSoapClient(LibraryWebServiceSoapClient.EndpointConfiguration
                    .LibraryWebServiceSoap12));

            services.Add(serviceDescriptor);
            return services;
        }
    }
}
