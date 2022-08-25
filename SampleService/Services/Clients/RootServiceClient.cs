using SampleService.Interfaces;

namespace SampleService.Services.Clients
{
    public class RootServiceClient : IRootServiceClient
    {
        private RootServiceNamespace.RootServiceClient _httpClient;

        public RootServiceClient(HttpClient httpClient)
        {
            _httpClient = new RootServiceNamespace.RootServiceClient("http://localhost:5241", httpClient);
        }

        public RootServiceNamespace.RootServiceClient Client => _httpClient;

        public async Task<ICollection<RootServiceNamespace.WeatherForecast>> Get()
        {
            return await _httpClient.GetWeatherForecastAsync();
        }
    }
}
