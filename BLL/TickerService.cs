using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TickerService : ITickerService
    {
        private readonly IYahooAPIService _yahooAPIService;

        public TickerService(IYahooAPIService yahooAPIService)
        {
            _yahooAPIService = yahooAPIService;
        }
        
        public async Task<string> GetTickerInformation(string ticker)
        {
            var yahooTickerInfo = await _yahooAPIService.GetStockInformationForTicker(ticker);
            return yahooTickerInfo.StockName;
        }
    }
}
