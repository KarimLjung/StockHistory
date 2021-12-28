
public class YahooStockInfo
{
    public QuoteResponse quoteResponse { get; set; }
}

public class QuoteResponse
{
    public Result[] result { get; set; }
    public object error { get; set; }
}

public class Result
{
    public string language { get; set; }
    public string region { get; set; }
    public string quoteType { get; set; }
    public string quoteSourceName { get; set; }
    public bool triggerable { get; set; }
    public string currency { get; set; }
    public string exchange { get; set; }
    public string decimalName { get; set; }
    public string messageBoardId { get; set; }
    public string exchangeTimezoneName { get; set; }
    public string exchangeTimezoneShortName { get; set; }
    public decimal gmtOffSetMilliseconds { get; set; }
    public string market { get; set; }
    public bool esgPopulated { get; set; }
    public string marketState { get; set; }
    public decimal regularMarketChange { get; set; }
    public decimal regularMarketChangePercent { get; set; }
    public decimal regularMarketTime { get; set; }
    public decimal regularMarketPrice { get; set; }
    public decimal regularMarketDayHigh { get; set; }
    public string regularMarketDayRange { get; set; }
    public decimal regularMarketDayLow { get; set; }
    public decimal regularMarketVolume { get; set; }
    public decimal regularMarketPreviousClose { get; set; }
    public decimal bid { get; set; }
    public decimal ask { get; set; }
    public decimal bidSize { get; set; }
    public decimal askSize { get; set; }
    public string fullExchangeName { get; set; }
    public string financialCurrency { get; set; }
    public decimal regularMarketOpen { get; set; }
    public decimal averageDailyVolume3Month { get; set; }
    public decimal averageDailyVolume10Day { get; set; }
    public decimal fiftyTwoWeekLowChange { get; set; }
    public decimal fiftyTwoWeekLowChangePercent { get; set; }
    public string fiftyTwoWeekRange { get; set; }
    public decimal fiftyTwoWeekHighChange { get; set; }
    public decimal fiftyTwoWeekHighChangePercent { get; set; }
    public decimal fiftyTwoWeekLow { get; set; }
    public decimal fiftyTwoWeekHigh { get; set; }
    public decimal dividendDate { get; set; }
    public decimal earningsTimestamp { get; set; }
    public decimal earningsTimestampStart { get; set; }
    public decimal earningsTimestampEnd { get; set; }
    public decimal trailingAnnualDividendRate { get; set; }
    public decimal trailingPE { get; set; }
    public decimal trailingAnnualDividendYield { get; set; }
    public decimal epsTrailingTwelveMonths { get; set; }
    public decimal epsForward { get; set; }
    public decimal epsCurrentYear { get; set; }
    public decimal priceEpsCurrentYear { get; set; }
    public decimal sharesOutstanding { get; set; }
    public decimal bookValue { get; set; }
    public decimal fiftyDayAverage { get; set; }
    public decimal fiftyDayAverageChange { get; set; }
    public decimal fiftyDayAverageChangePercent { get; set; }
    public decimal twoHundredDayAverage { get; set; }
    public decimal twoHundredDayAverageChange { get; set; }
    public decimal twoHundredDayAverageChangePercent { get; set; }
    public decimal marketCap { get; set; }
    public decimal forwardPE { get; set; }
    public decimal priceToBook { get; set; }
    public decimal firstTradeDateMilliseconds { get; set; }
    public decimal priceHdecimal { get; set; }
    public decimal preMarketChange { get; set; }
    public decimal preMarketChangePercent { get; set; }
    public decimal preMarketTime { get; set; }
    public decimal preMarketPrice { get; set; }
    public string shortName { get; set; }
    public decimal sourcedecimalerval { get; set; }
    public decimal exchangeDataDelayedBy { get; set; }
    public decimal pageViewGrowthWeekly { get; set; }
    public string averageAnalystRating { get; set; }
    public bool tradeable { get; set; }
    public string displayName { get; set; }
    public string symbol { get; set; }
}
