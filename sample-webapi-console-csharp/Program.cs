using System;
using System.Linq;

namespace SymbolSuggestDemo
{
    static class Program
    {
        static void Main()
        {
            var api = new TradeStationWebApi(
                Properties.Settings.Default.APIKey,
                Properties.Settings.Default.APISecret,
                Properties.Settings.Default.Environment,
                Properties.Settings.Default.RedirectUri);

            // Get Accounts
            var accounts = api.GetUserAccounts().ToArray();
            foreach (var account in accounts)
            {
                Console.WriteLine("Key: {0}\t\tName: {1}\t\tType: {2}\t\tTypeDescription: {3}",
                                  account.Key, account.Name, account.Type, account.TypeDescription);
            }

            // Get Orders
            var orders = api.GetOrders(accounts.Select(account => account.Key)).ToArray();
            if (!orders.Any())
            {
                Console.WriteLine("No Orders to Display");
            }
            else
            {
                Console.WriteLine("Order Requests:");
                foreach (var orderDetail in orders)
                {
                    Console.WriteLine("Account ID: {0}\t\tOrder ID: {1}\t\tSymbol: {2}\t\tQuantity: {3}\t\tStatus: {4}",
                                      orderDetail.AccountID, orderDetail.OrderID, orderDetail.Symbol,
                                      orderDetail.Quantity, orderDetail.Status);
                }
            }

            // SymbolSuggest
            Console.Write("Enter symbol: ");
            var suggestText = Console.ReadLine();
            var symbols = api.SymbolSuggest(suggestText);
            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach (var symbol in symbols)
            {
                Console.WriteLine(symbol.Name);
            }

            // GetQuoteChanges
            Console.Write("Enter symbol to stream: ");
            var symbolsCommaDelimited = Console.ReadLine();
            api.GetQuoteChanges(symbolsCommaDelimited);
            Console.ReadLine();
        }
    }
}
