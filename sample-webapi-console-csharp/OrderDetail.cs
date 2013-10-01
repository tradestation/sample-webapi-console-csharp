namespace SymbolSuggestDemo
{
    public class OrderDetail
    {
        public string AccountID { get; set; }
        public string AdvancedOptions { get; set; }
        public string Alias { get; set; }
        public string AssetType { get; set; }
        public decimal CommissionFee { get; set; }
        public string ContractExpireDate { get; set; }
        public decimal ConversionRate { get; set; }
        public string Country { get; set; }
        public string Denomination { get; set; }
        public string DisplayName { get; set; }
        public string Duration { get; set; }
        public int ExecuteQuantity { get; set; }
        public string FilledCanceled { get; set; }
        public string FilledPriceText { get; set; }
        public string GroupName { get; set; }
        public Leg[] Legs { get; set; }
        public string LimitPriceText { get; set; }
        public int OrderID { get; set; }
        public int Originator { get; set; }
        public int Quantity { get; set; }
        public int QuantityLeft { get; set; }
        public string RejectReason { get; set; }
        public string Routing { get; set; }
        public string Spread { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string StopPriceText { get; set; }
        public string Symbol { get; set; }
        public string TimeStamp { get; set; }
        public string TriggeredBy { get; set; }
        public string Type { get; set; }
    }
}