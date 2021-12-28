using BLL;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace AL
{
    public class YahooAPIService : IYahooAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string yahooAPIhostname = "https://yfapi.net/v6/finance/quote";
        public YahooAPIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TickerInfo> GetStockInformationForTicker(string ticker)
        {
            var parameters = "region=US&lang=en&symbols=AAPL%2CBTC-USD%2CEURUSD%3DX";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("X-API-KEY", "oF7fRrwMxK5xcFmFlUKzl44aYDt7HpUd19AUstAj");
            var query = new Dictionary<string, string>()
            {
                {"region", "US" },
                {"lang", "en" },
                {"symbols", "AAPL" }
            };

            var responseMessage = await client.GetAsync(
                QueryHelpers.AddQueryString(yahooAPIhostname, query));

            if (responseMessage.IsSuccessStatusCode)
            {
                var tickerString =
                    await responseMessage.Content.ReadAsStringAsync();
                var yahooStockInfo = JsonSerializer.Deserialize<YahooStockInfo>(tickerString);
                var tickerInfo = yahooStockInfo?.quoteResponse?.result?.Select(f => new TickerInfo { StockName = f.shortName, Id = Guid.NewGuid().ToString()} ).First();
                
                return tickerInfo;
            }

            return null;
        }
    }
}