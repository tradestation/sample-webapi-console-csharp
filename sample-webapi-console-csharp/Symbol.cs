using System;

namespace SymbolSuggestDemo
{
    class Symbol
    {
        public string Category { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public object Error { get; set; }
        public string Exchange { get; set; }
        public int ExchangeID { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FutureType { get; set; }
        public int LotSize { get; set; }
        public int MinMove { get; set; }
        public string Name { get; set; }
        public string OptionType { get; set; }
        public int PointValue { get; set; }
        public string Root { get; set; }
        public int StrikePrice { get; set; }
    }
}
