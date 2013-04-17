using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymbolSuggestDemo
{
    internal class Quote
    {
        public string Symbol { get; set; }
        public decimal ForexWinFactor { get; set; }
        public decimal ForexLoseFactor { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public decimal Last { get; set; }
        public decimal PreviousClose { get; set; }
        public string Exchange { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public int Volume { get; set; }
        public int BidSize { get; set; }
        public int AskSize { get; set; }
        public decimal High52Week { get; set; }
        public decimal Low52Week { get; set; }
        public int CurrencyCode { get; set; }
        public decimal StrikePrice { get; set; }
        public int DailyOpenInterest { get; set; }
        public string SymbolRoot { get; set; }
        public int ContractExpireDate { get; set; }
        public int ExchangeID { get; set; }
        public int DisplayType { get; set; }
        public decimal PointValue { get; set; }
        public decimal MinMove { get; set; }
        public decimal Close { get; set; }
        public int PreviousVolume { get; set; }
        public string NameExt { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string CountryCode { get; set; }
        public string AssetType { get; set; }
        public string Currency { get; set; }
        public decimal NetChange { get; set; }
        public decimal NetChangePct { get; set; }
    }
}
