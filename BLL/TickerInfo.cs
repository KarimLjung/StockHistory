using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TickerInfo
    {
        
        public string Ticker { get; set; }
        public string StockName { get; set; }
        public string Id { get; set; }
        public decimal LatestPrice { get; set; }
        public DateTime LatestPriceDate { get; set; }
    }
}
