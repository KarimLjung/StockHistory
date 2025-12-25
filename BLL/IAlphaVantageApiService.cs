namespace BLL
{
    public interface IAlphaVantageApiService
    {
        Task<TickerInfo> GetStockInformationForTicker(string ticker);
        Task<IEnumerable<TickerPrice>> GetStockInformationForTickerRange(string ticker, DateTime startDate, DateTime endDate);
    }
}
