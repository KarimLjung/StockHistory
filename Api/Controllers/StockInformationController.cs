using Microsoft.AspNetCore.Mvc;
using BLL;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class StockInformationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<StockInformationController> _logger;
        private readonly ITickerService _tickerService;

        public StockInformationController(ILogger<StockInformationController> logger,
            ITickerService tickerService)
        {
            _logger = logger;
            this._tickerService = tickerService;
        }

        [HttpGet]
        [Route("/TickerInformation/{ticker}")]
        public async Task<TickerInfo> GetTickerInformation(string ticker)
        {
            return await _tickerService.GetTickerInformation(ticker).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("/TickerInformations/{ticker}")]
        public async Task<ActionResult<IEnumerable<TickerPrice>>> GetTickerInformations(
            string ticker,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (startDate == default || endDate == default)
            {
                return BadRequest("startDate and endDate are required (YYYY-MM-DD).");
            }

            if (startDate > endDate)
            {
                return BadRequest("startDate must be on or before endDate.");
            }

            var tickerInfos = await _tickerService
                .GetTickerInformations(ticker, startDate, endDate)
                .ConfigureAwait(false);
            return Ok(tickerInfos);
        }

        [HttpPost]
        [Route("/DoNothing/{ticker}")]
        public async Task<string> GetNothing(string ticker)
        {
            var tickerInfos = "Hello world";
            return tickerInfos;
        }

    }
}
