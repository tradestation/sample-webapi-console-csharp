using System;
using System.Collections.Generic;
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
            var symbolsSuggested = api.SymbolSuggest(suggestText);
            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach (var symbol in symbolsSuggested)
            {
                Console.WriteLine(symbol.Name);
            }

            // Get Quotes
            Console.Write("Provide a list of symbols to retrieve quotes (example: MSFT,GOOG): ");
            var symbols = Console.ReadLine();
            var quotes = api.GetQuotes(symbols.Split(',')).ToArray();
            foreach (var quote in quotes)
            {
                Console.WriteLine("Symbol: {0}\t\tLast: {1}\t\tLastPriceDisplay: {2}\t\t" +
                                  "CountryCode: {3}\t\tCurrency: {4}\t\tAsset Type: {5}", quote.Symbol, quote.Last.ToString("C"),
                                  quote.LastPriceDisplay, quote.CountryCode, quote.Currency, quote.AssetType);
            }

            // New-up an order
            var order = new Order(quotes.First().Description, null, quotes.First().AssetType.ToOrderAssetType(),
                                  quotes.First().Symbol, "1",
                                  quotes.First().LastPriceDisplay, null, "Limit", "Intelligent", "DAY",
                                  accounts.Select(account => account.Key).First(), "", "buy", true, null,
                                  new List<GroupOrder>());
            Console.WriteLine("Trying to place an order of {0} share of {1} at {2}", order.Quantity, order.Symbol,
                              order.LimitPrice);

            // Get Order Confirmation
            var confirmation = api.GetConfirmations(order).First();
            if (confirmation.StatusCode != null && confirmation.StatusCode.Equals("400"))
            {
                Console.WriteLine("Message: {0}\t\tStatus Code: {1}", confirmation.Message, confirmation.StatusCode);
            }
            else
            {
                Console.WriteLine("SummaryMessage: {0}", confirmation.SummaryMessage);
            }

            // Place an Order
            var orderResults = api.PlaceOrder(order).ToArray();
            Console.WriteLine("Message: {0}\t\tStatus Code: {1}", orderResults.First().Message,
                              orderResults.First().StatusCode);

            // Check Order Status
            orders = api.GetOrders(accounts.Select(account => account.Key)).ToArray();
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

            // GetQuoteChanges
            Console.Write("Enter symbol to stream: ");
            var symbolsCommaDelimited = Console.ReadLine();
            api.GetQuoteChanges(symbolsCommaDelimited);
            Console.ReadLine();
        }
    }
}
