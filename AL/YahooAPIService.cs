using BLL;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AL
{
    public class YahooAPIService : IYahooAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<YahooAPIService> _logger;

        private const string YahooApiHost = "https://yfapi.net/v7/finance/options/";
        public YahooAPIService(IHttpClientFactory httpClientFactory, ILogger<YahooAPIService> logger)
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
                client.DefaultRequestHeaders.Add("X-API-KEY", "oF7fRrwMxK5xcFmFlUKzl44aYDt7HpUd19AUstAj");

                var requestUrl = $"{YahooApiHost}{ticker}";
                var responseMessage = await client.GetAsync(requestUrl);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var tickerString = await responseMessage.Content.ReadAsStringAsync();
                    var yahooStockInfoOptions = JsonSerializer.Deserialize<YahooStockInfoOptions>(tickerString);
                    TickerInfo? tickerInfo = CreateTickerInfo(ticker, yahooStockInfoOptions);

                    return tickerInfo;
                }

                var errorBody = await responseMessage.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "Yahoo API request failed for {Ticker}. Status: {StatusCode}. Body: {Body}",
                    ticker,
                    responseMessage.StatusCode,
                    errorBody);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yahoo API request threw for {Ticker}", ticker);
                return null;
            }
        }

        private static TickerInfo? CreateTickerInfo(string ticker, YahooStockInfoOptions? yahooStockInfo)
        {
            return yahooStockInfo?.optionChain.result?.
                                Select(f => new TickerInfo
                                {
                                    Ticker = ticker,
                                    StockName = f.underlyingSymbol,
                                    Id = Guid.NewGuid().ToString(),
                                    LatestPrice = f.quote.regularMarketPrice,
                                    LatestPriceDate = DateTime.UtcNow
                                }).First();
        }
    }
}
