using System;

namespace BLL
{
    public class TickerPrice
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }
}
