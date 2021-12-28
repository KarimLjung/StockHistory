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
        private readonly IStockRepository _stockRepository;

        public TickerService(IYahooAPIService yahooAPIService, IStockRepository stockRepository)
        {
            _yahooAPIService = yahooAPIService;
            this._stockRepository = stockRepository;
        }
        
        public async Task<string> GetTickerInformation(string ticker)
        {
            var tickerInfo = await _yahooAPIService.GetStockInformationForTicker(ticker);
            _stockRepository.CreateStockInfo(tickerInfo);

            return tickerInfo.StockName;
        }
    }
}
