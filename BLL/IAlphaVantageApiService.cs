namespace BLL
{
    public interface IAlphaVantageApiService
    {
        Task<TickerInfo> GetStockInformationForTicker(string ticker);
    }
}
