using BLL;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace AL
{
    public class YahooAPIService : IYahooAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        string yahooAPIhostname = "https://yfapi.net/v7/finance/options/";
        public YahooAPIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TickerInfo> GetStockInformationForTicker(string ticker)
        {
            yahooAPIhostname += ticker;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("X-API-KEY", "oF7fRrwMxK5xcFmFlUKzl44aYDt7HpUd19AUstAj");

            var unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var query = new Dictionary<string, string>()
            {
                {"date", unixTimestamp.ToString() }
            };

            var responseMessage = await client.GetAsync(
                QueryHelpers.AddQueryString(yahooAPIhostname, query));

            if (responseMessage.IsSuccessStatusCode)
            {
                var tickerString =
                    await responseMessage.Content.ReadAsStringAsync();
                var yahooStockInfoOptions = JsonSerializer.Deserialize<YahooStockInfoOptions>(tickerString);
                TickerInfo? tickerInfo = CreateTickerInfo(ticker, yahooStockInfoOptions);

                return tickerInfo;
            }

            return null;
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