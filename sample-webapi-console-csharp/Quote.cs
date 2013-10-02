namespace SymbolSuggestDemo
{
    public class Quote
    {
        public string Symbol { get; set; }
        public string SymbolRoot { get; set; }
        public decimal ForexWinFactor { get; set; }
        public decimal ForexLoseFactor { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public string AssetType { get; set; }
        public string Exchange { get; set; }
        public bool FractionalDisplay { get; set; }
        public int DisplayType { get; set; }
        public decimal Open { get; set; }
        public string OpenPriceDisplay { get; set; }
        public decimal High { get; set; }
        public string HighPriceDisplay { get; set; }
        public decimal Low { get; set; }
        public string LowPriceDisplay { get; set; }
        public decimal PreviousClose { get; set; }
        public string PreviousClosePriceDisplay { get; set; }
        public decimal Last { get; set; }
        public string LastPriceDisplay { get; set; }
        public decimal Ask { get; set; }
        public string AskPriceDisplay { get; set; }
        public int AskSize { get; set; }
        public decimal Bid { get; set; }
        public string BidPriceDisplay { get; set; }
        public int BidSize { get; set; }
        public decimal NetChange { get; set; }
        public decimal NetChangePct { get; set; }
        public decimal High52Week { get; set; }
        public string High52WeekPriceDisplay { get; set; }
        public decimal Low52Week { get; set; }
        public string Low52WeekPriceDisplay { get; set; }
        public int Volume { get; set; }
        public int PreviousVolume { get; set; }
        public string Currency { get; set; }
        public string CountryCode { get; set; }
        public decimal StrikePrice { get; set; }
        public string StrikePriceDisplay { get; set; }
        public string NameExt { get; set; }
        public decimal MinMove { get; set; }
        public decimal PointValue { get; set; }
        public decimal Close { get; set; }
        public string ClosePriceDisplay { get; set; }
        public string Error { get; set; }
        public int DailyOpenInterest { get; set; }
        public bool IsDelayed { get; set; }                
        public int CurrencyCode { get; set; }
        public int ContractExpireDate { get; set; }
        public int ExchangeID { get; set; }        
    }

    public static class QuoteExtensions
    {
        public static string ToOrderAssetType(this string quoteAssetType)
        {
            if (quoteAssetType.Equals("STOCK") || quoteAssetType.Equals("EQUITY"))
            {
                return "EQ";
            }
            if (quoteAssetType.Equals("FUTURE"))
            {
                return "FU";
            }
            if (quoteAssetType.Equals("FOREX"))
            {
                return "FX";
            }
            if (quoteAssetType.Equals("OPTION"))
            {
                return "OP";
            }
            return quoteAssetType;
        }
    }
}
