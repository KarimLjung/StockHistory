using BLL;

namespace AL
{
    public class YahooAPIService : IYahooAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string yahooAPIhostname = "yfapi.net/v6/finance/quote";
        public YahooAPIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetStockInformationForTicker(string ticker)
        {
            var parameters = "region=US&lang=en&symbols=AAPL%2CBTC-USD%2CEURUSD%3DX";
            var client = _httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            yahooAPIhostname + parameters)
            {
                Headers =
            {
                { "accept", "application/json" },
                { "X-API-KEY", "oF7fRrwMxK5xcFmFlUKzl44aYDt7HpUd19AUstAj" }
            }
            };

            var responseMessage = await client.SendAsync(httpRequestMessage);

            if (responseMessage.IsSuccessStatusCode)
            {
                var tickerString =
                    await responseMessage.Content.ReadAsStringAsync();
                return tickerString;
            }

            return String.Empty;
        }
    }
}