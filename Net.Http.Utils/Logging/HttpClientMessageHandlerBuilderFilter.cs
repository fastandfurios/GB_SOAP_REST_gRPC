using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Http.Utils.Options;

namespace Net.Http.Utils.Logging
{
    internal class HttpClientMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        public HttpClientMessageHandlerBuilderFilter(ILoggerFactory loggerFactory,
            IOptions<HttpClientLoggingOptions> options)
        {
            LoggerFactory = loggerFactory;
            Options = options;
        }

        public ILoggerFactory LoggerFactory { get; }

        public IOptions<HttpClientLoggingOptions> Options { get; }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return (HttpMessageHandlerBuilder builder) =>
            {
                // call next handler 
                next(builder);

                // create logger 
                var loggerName = !string.IsNullOrEmpty(builder.Name) ? builder.Name : "Default";
                var logger = LoggerFactory.CreateLogger($"Net.Http.Utils.HttpClient.{loggerName}");

                // create and add handler 
                var loggingHttpMessageHandler = new LoggingHttpMessageHandler(logger, Options);
                builder.AdditionalHandlers.Insert(0, loggingHttpMessageHandler);
            };
        }

    }
}
