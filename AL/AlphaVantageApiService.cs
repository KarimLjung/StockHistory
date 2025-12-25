using BLL;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace AL
{
    public class AlphaVantageApiService : IAlphaVantageApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AlphaVantageApiService> _logger;

        private const string AlphaVantageApiHost = "https://www.alphavantage.co/query";
        private const string AlphaVantageApiKey = "H2Q4AR4PGTGMV4D0";
        public AlphaVantageApiService(IHttpClientFactory httpClientFactory, ILogger<AlphaVantageApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<TickerInfo> GetStockInformationForTicker(string ticker)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var requestUrl =
                    $"{AlphaVantageApiHost}?function=GLOBAL_QUOTE&symbol={ticker}&apikey={AlphaVantageApiKey}";
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
    }
}
