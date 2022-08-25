using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Http.Utils.Options;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace Net.Http.Utils.Logging
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        public LoggingHttpMessageHandler(
            ILogger logger,
            IOptions<HttpClientLoggingOptions> options)
        {
            Logger = logger;
            Options = options.Value;
        }

        public ILogger Logger { get; }

        public HttpClientLoggingOptions Options { get; }

        [DebuggerStepThrough]
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // generate correlation id
            var correlationId = Guid.NewGuid();

            // request tracing
            if (Logger.IsEnabled(LogLevel.Information))
            {
                // tracing request
                var requestText = new StringBuilder();
                requestText.AppendLine($"Request #{correlationId}");

                // parse URI
                var requestUri = request.RequestUri;
                requestText.AppendLine($"Host: {requestUri.Host}");
                requestText.AppendLine($"Path: {requestUri.AbsolutePath}");
                requestText.AppendLine($"QueryString: {requestUri.Query}");
                requestText.AppendLine($"Method: {request.Method}");
                requestText.AppendLine($"Scheme: {requestUri.Scheme}");

                // tracing request headers
                foreach (var header in request.Headers)
                    foreach (var value in header.Value)
                        requestText.AppendLine($"{header.Key}: {value}");
                
                // tracing request contents 
                if (request.Content != null)
                {
                    if (CheckContentType(request.Content.Headers.ContentType))
                    {
                        // read body
                        var requestBody = await request.Content.ReadAsStringAsync();

                        // truncate too long bodies
                        if (requestBody != null && requestBody.Length >= Options.MaxBodyLength)
                        {
                            requestBody = requestBody.Substring(0, Options.MaxBodyLength) + "... <Truncated>";
                        }

                        // append log
                        requestText.AppendLine($"RequestBody:");
                        requestText.AppendLine(requestBody);
                    }
                    else
                    {
                        // append log 
                        requestText.AppendLine($"RequestBody: <Not Logged>");
                    }
                }

                // tracing output
                Logger.LogInformation(requestText.ToString());
            }

            // Measure execution time
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // execute request 
            HttpResponseMessage response;
            try
            {
                // execute request
                response = await base.SendAsync(request, cancellationToken);

                // stop measuring time 
                stopwatch.Stop();
            }
            catch (Exception exception)
            {
                // stop measuring time 
                stopwatch.Stop();

                // tracing response
                var responseText = new StringBuilder();
                responseText.AppendLine($"Response #{correlationId}");
                responseText.AppendLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
                responseText.AppendLine($"Exception: {exception.Message}");

                // tracing output
                Logger.LogInformation(responseText.ToString());

                // re-throw
                throw;
            }

            // response tracing 
            if (Logger.IsEnabled(LogLevel.Information))
            {
                // tracing response
                var responseText = new StringBuilder();
                responseText.AppendLine($"Response #{correlationId}");
                responseText.AppendLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
                responseText.AppendLine($"StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}");

                // get response headers
                IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers = response.Headers;

                // add content response headers
                if (response.Content != null && response.Content.Headers != null)
                    headers = headers.Concat(response.Content.Headers);

                // tracing response headers
                foreach (var header in headers.OrderBy(item => item.Key))
                    foreach (var value in header.Value)
                        responseText.AppendLine($"{header.Key}: {value}");
                
                // tracing response contents
                if (response.Content != null)
                {
                    if (IsTextContent(response.Content.Headers.ContentType))
                    {
                        // fetch body 
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // truncate too long bodies
                        if (responseBody != null && responseBody.Length >= Options.MaxBodyLength)
                        {
                            responseBody = responseBody.Substring(0, Options.MaxBodyLength) + "... <Truncated>";
                        }

                        // append log 
                        responseText.AppendLine($"ResponseBody:");
                        responseText.AppendLine(responseBody);
                    }
                    else
                    {
                        // append log 
                        responseText.AppendLine($"ResponseBody: <Not Logged>");
                    }
                }

                // tracing output
                Logger.LogInformation(responseText.ToString());
            }

            return response;
        }

        private bool CheckContentType(MediaTypeHeaderValue contentType)
        {
            // for multipart form data we allow body logging only
            // when explicitly specified
            if (contentType != null && contentType.MediaType.Contains("multipart/form-data"))
            {
                return Options.LogMultipartFormData;
            }

            // allow body logging 
            return true;
        }

        private bool IsTextContent(MediaTypeHeaderValue contentType)
        {
            return contentType != null &&
                (contentType.MediaType.EndsWith("/json") ||
                 contentType.MediaType.EndsWith("/xml") ||
                 contentType.MediaType.StartsWith("text/"));
        }
    }
}