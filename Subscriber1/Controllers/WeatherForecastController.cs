using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Subscriber1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Topic("pubsub", "myorders2")]
        [HttpPost("subscriber2")]
        public async Task<ActionResult> Subscribe2(MyOrder order)
        {
            //using var client = new DaprClientBuilder().Build();
            //await client.PublishEventAsync("pubsub", "myorders2", order);
            Console.WriteLine($"Subscribed order with order id {order.MyOrderId} and name {order.MyOrderName}");
            await Task.Run(() =>
            {
                Console.WriteLine($"Subscribed to order with order id {order.MyOrderId} and name {order.MyOrderName}");
            });
            return Ok(order);
        }
    }
}