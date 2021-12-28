namespace BLL
{
    public interface IYahooAPIService
    {
        Task<TickerInfo> GetStockInformationForTicker(string ticker);
    }
}
