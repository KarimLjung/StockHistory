using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TickerService : ITickerService
    {
        private readonly IAlphaVantageApiService _alphaVantageApiService;
        private readonly IStockRepository _stockRepository;

        public TickerService(IAlphaVantageApiService alphaVantageApiService, IStockRepository stockRepository)
        {
            _alphaVantageApiService = alphaVantageApiService;
            this._stockRepository = stockRepository;
        }

        public async Task<TickerInfo> GetTickerInformation(string ticker)
        {
            var tickerInfo = await _alphaVantageApiService.GetStockInformationForTicker(ticker);
            //_stockRepository.CreateStockInfo(tickerInfo);

            return tickerInfo;
        }

        public async Task<IEnumerable<TickerPrice>> GetTickerInformations(string ticker, DateTime startDate, DateTime endDate)
        {
            return await _alphaVantageApiService.GetStockInformationForTickerRange(ticker, startDate, endDate);
        }
    }
}
