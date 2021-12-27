namespace BLL
{
    public interface IYahooAPIService
    {
        Task<string> GetStockInformationForTicker(string ticker);
    }
}
