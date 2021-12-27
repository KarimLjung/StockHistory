using Microsoft.AspNetCore.Mvc;
using BLL;

namespace Api.Controllers
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
        private readonly ITickerService _tickerService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ITickerService tickerService)
        {
            _logger = logger;
            this._tickerService = tickerService;
        }

        [HttpGet(Name = "TickerInformation")]
        public async Task<string> GetTickerInformation(string ticker)
        {
            var tickerString = await _tickerService.GetTickerInformation(ticker).ConfigureAwait(false);
            return tickerString;
        }
    }
}