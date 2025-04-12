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
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
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
                new ApiResponse(
                    System.Net.HttpStatusCode.OK,
                        Enumerable.Range(1, 5).Select(index => new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                        }).ToArray()
                    );
        }

        [HttpPost]
        public ApiResponse AddWeatherForecast(string weather)
        {
            Summaries.Add(weather);
            return new ApiResponse(System.Net.HttpStatusCode.OK);
        }
    }
}
