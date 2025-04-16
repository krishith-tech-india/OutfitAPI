using Core;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly List<string> Summaries = new()
        {
            "Freezing", "Bracing", "Chilly", "Cool"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ApiResponse GetWeatherForecast()
        {
            return
                new ApiResponse(System.Net.HttpStatusCode.OK, Summaries);
        }

        [HttpGet("{id}")]
        public ApiResponse GetWeatherForecast(int id)
        {
            if (id >= Summaries.Count)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "No SummaryFound");
            }

            return new ApiResponse(System.Net.HttpStatusCode.OK, Summaries[id]);
        }

        [HttpPost]
        public ApiResponse AddWeatherForecast(string weather)
        {
            Summaries.Add(weather);
            return new ApiResponse(System.Net.HttpStatusCode.OK);
        }
    }
}