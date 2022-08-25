using Microsoft.AspNetCore.Mvc;
using SampleService.Interfaces;

namespace SampleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private IRootServiceClient _rootServiceClient;

        public WeatherForecastController(
            IRootServiceClient rootServiceClient,
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _rootServiceClient = rootServiceClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<RootServiceNamespace.WeatherForecast>>> Get()
        {
            _logger.LogInformation("WeatherForecastController >>> START  GetWeatherForecast");
            var res = await _rootServiceClient.Get();
            _logger.LogInformation("WeatherForecastController >>> END  GetWeatherForecast");
            return Ok(res);
        }
    }
}