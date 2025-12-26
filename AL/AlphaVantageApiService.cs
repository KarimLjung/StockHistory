using System.Text.Json;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using BLL;

namespace AL
{
    public class AlphaVantageApiService : IAlphaVantageApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AlphaVantageApiService> _logger;
        private readonly AlphaVantageOptions _options;
        private const string AlphaVantageApiHost = "https://www.alphavantage.co/query";
        public AlphaVantageApiService(
            IHttpClientFactory httpClientFactory, 
            ILogger<AlphaVantageApiService> logger,
            IOptions<AlphaVantageOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<TickerInfo> GetStockInformationForTicker(string ticker)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var requestUrl =
                    // $"{AlphaVantageApiHost}?function=GLOBAL_QUOTE&symbol={ticker}&apikey={_options.ApiKey}";
                 $"{AlphaVantageApiHost}?function=GLOBAL_QUOTE&symbol={ticker}&apikey={_options.ApiKey}";
                var responseMessage = await client.GetAsync(requestUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var tickerString = await responseMessage.Content.ReadAsStringAsync();
                    TickerInfo? tickerInfo = CreateTickerInfoFromAlphaVantage(ticker, tickerString);
                    if (tickerInfo == null)
                    {
                        _logger.LogWarning(
                            "AlphaVantage response missing expected fields for {Ticker}. Body: {Body}",
                            ticker,
                            tickerString);
                    }

                    return tickerInfo;
                }

                var errorBody = await responseMessage.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "AlphaVantage API request failed for {Ticker}. Status: {StatusCode}. Body: {Body}",
                    ticker,
                    responseMessage.StatusCode,
                    errorBody);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AlphaVantage API request threw for {Ticker}", ticker);
                return null;
            }
        }

        public async Task<IEnumerable<TickerPrice>> GetStockInformationForTickerRange(
            string ticker,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var requestUrl =
                    $"{AlphaVantageApiHost}?function=TIME_SERIES_DAILY_ADJUSTED&symbol={ticker}&outputsize=full&apikey={_options.ApiKey}";
                var responseMessage = await client.GetAsync(requestUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var payload = await responseMessage.Content.ReadAsStringAsync();
                    var prices = CreateTickerPricesFromAlphaVantage(ticker, payload, startDate, endDate);
                    if (!prices.Any())
                    {
                        _logger.LogWarning(
                            "AlphaVantage response returned no prices for {Ticker} between {StartDate} and {EndDate}. Body: {Body}",
                            ticker,
                            startDate,
                            endDate,
                            payload);
                    }

                    return prices;
                }

                var errorBody = await responseMessage.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "AlphaVantage API request failed for {Ticker}. Status: {StatusCode}. Body: {Body}",
                    ticker,
                    responseMessage.StatusCode,
                    errorBody);
                return Enumerable.Empty<TickerPrice>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AlphaVantage API request threw for {Ticker}", ticker);
                return Enumerable.Empty<TickerPrice>();
            }
        }

        private static TickerInfo? CreateTickerInfoFromAlphaVantage(string ticker, string payload)
        {
            using var doc = JsonDocument.Parse(payload);
            if (!doc.RootElement.TryGetProperty("Global Quote", out var quote))
            {
                return null;
            }

            if (!quote.TryGetProperty("05. price", out var priceElement))
            {
                return null;
            }

            var priceText = priceElement.GetString();
            if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                return null;
            }

            DateTime priceDate = DateTime.UtcNow;
            if (quote.TryGetProperty("07. latest trading day", out var dateElement))
            {
                var dateText = dateElement.GetString();
                if (DateTime.TryParse(dateText, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                {
                    priceDate = parsed;
                }
            }

            var symbol = quote.TryGetProperty("01. symbol", out var symbolElement)
                ? symbolElement.GetString()
                : null;

            var resolvedSymbol = symbol ?? ticker;
            return new TickerInfo
            {
                Ticker = resolvedSymbol,
                StockName = resolvedSymbol,
                Id = Guid.NewGuid().ToString(),
                LatestPrice = price,
                LatestPriceDate = priceDate
            };
        }

        private IEnumerable<TickerPrice> CreateTickerPricesFromAlphaVantage(
            string ticker,
            string payload,
            DateTime startDate,
            DateTime endDate)
        {
            
            using var doc = JsonDocument.Parse(payload);
            if (!doc.RootElement.TryGetProperty("Time Series (Daily)", out var series))
            {
                return Enumerable.Empty<TickerPrice>();
            }

            var start = startDate.Date;
            var end = endDate.Date;
            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }

            var allPrices = new List<TickerPrice>();
            foreach (var day in series.EnumerateObject())
            {
                if (!DateTime.TryParse(day.Name, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dayDate))
                {
                    continue;
                }

                if (!day.Value.TryGetProperty("4. close", out var closeElement))
                {
                    continue;
                }

                var closeText = closeElement.GetString();
                if (!decimal.TryParse(closeText, NumberStyles.Any, CultureInfo.InvariantCulture, out var close))
                {
                    continue;
                }

                allPrices.Add(new TickerPrice
                {
                    Ticker = ticker,
                    Date = dayDate.Date,
                    Close = close
                });
            }

            if (allPrices.Count == 0)
            {
                return Enumerable.Empty<TickerPrice>();
            }

            var minDate = allPrices.Min(p => p.Date);
            var maxDate = allPrices.Max(p => p.Date);

            if (start < minDate)
            {
                start = minDate;
            }
            if (end < minDate)
            {
                end = minDate;
            }
            if (start > maxDate)
            {
                start = maxDate;
            }
            if (end > maxDate)
            {
                end = maxDate;
            }
            if (start > end)
            {
                start = end;
            }
            _logger.LogWarning("Count of allPrices: " + allPrices.Count);

            return allPrices
                .Where(p => p.Date >= start && p.Date <= end)
                .OrderBy(p => p.Date);
        }
    }
}
