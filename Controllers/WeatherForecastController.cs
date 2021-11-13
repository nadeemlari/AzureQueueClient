using Azure.Storage.Queues;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AzureQueueClient.Controllers
{
    [ApiController]
    [Route("/")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly QueueClient client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , QueueClient client)
        {
            this.logger  =   logger;
            this.client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WeatherForecast weatherForecast)
        {
            var message = JsonSerializer.Serialize(weatherForecast);
            await client.SendMessageAsync(message,TimeSpan.FromSeconds(0),TimeSpan.FromSeconds(-1));
            return Ok();
        }
    }
}