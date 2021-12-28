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

        public StockRepository(StockInfoDbContext stockInfoDbContext)
        {
            this._stockInfoDbContext = stockInfoDbContext;
        }

        public void CreateStockInfo(TickerInfo tickerInfo)
        {
            _stockInfoDbContext.TickerInfo.Add(tickerInfo);
            _stockInfoDbContext.SaveChanges();
        }
    }
}
