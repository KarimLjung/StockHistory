using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StockRepository : IStockRepository
    {
        private readonly StockInfoDbContext _stockInfoDbContext;
        private readonly StockhistoryContextProcedures _stockhistoryContextProcedures;

        public StockRepository(StockInfoDbContext stockInfoDbContext, 
            StockhistoryContextProcedures stockhistoryContextProcedures)
        {
            this._stockInfoDbContext = stockInfoDbContext;
            this._stockhistoryContextProcedures = stockhistoryContextProcedures;
        }

        public void CreateStockInfo(TickerInfo tickerInfo)
        {
            _stockInfoDbContext.TickerInfo.Add(tickerInfo);
            _stockInfoDbContext.SaveChanges();
        }

        public async Task<IEnumerable<TickerInfos>> GetTickerInfoResults()
        {
            var tickerResults = await _stockhistoryContextProcedures.GetTickerInfoAsync("123");
            return tickerResults.Select(f => new TickerInfos { Id = f.Id });
        }
    }
}
