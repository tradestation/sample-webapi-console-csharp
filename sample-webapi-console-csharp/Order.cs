using System.Collections.Generic;

namespace SymbolSuggestDemo
{
    public class Order
    {
        public string Description { get; set; }
        public string OrderID { get; set; }
        public string AssetType { get; set; }
        public string Symbol { get; set; }
        public string Quantity { get; set; }
        public string LimitPrice { get; set; }
        public string StopPrice { get; set; }
        public string OrderType { get; set; }
        public string Route { get; set; }
        public string Duration { get; set; }
        public int AccountKey { get; set; }
        public string GTDDate { get; set; }
        public string TradeAction { get; set; }
        public bool IsDelayed { get; set; }
        public AdvancedOptions AdvancedOptions { get; set; }
        public IEnumerable<GroupOrder> OSOs { get; set; }

        public Order(string description, string orderID, string assetType, string symbol, string quantity, string limitPrice, string stopPrice,
                     string orderType, string route, string duration, int accountKey, string gtdDate, string tradeAction,
                     bool isDelayed, AdvancedOptions advancedOptions, IEnumerable<GroupOrder> osos)
        {
            Description = description;
            OrderID = orderID;
            AssetType = assetType;
            Symbol = symbol;
            Quantity = quantity;
            LimitPrice = limitPrice;
            StopPrice = stopPrice;
            OrderType = orderType;
            Route = route;
            Duration = duration;
            AccountKey = accountKey;
            GTDDate = gtdDate;
            IsDelayed = isDelayed;
            TradeAction = tradeAction;
            AdvancedOptions = advancedOptions;
            OSOs = osos;
        }
    }
}
