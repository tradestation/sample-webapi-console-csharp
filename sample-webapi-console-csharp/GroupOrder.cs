using System.Collections.Generic;

namespace SymbolSuggestDemo
{
    public class GroupOrder
    {
        public string Type { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}