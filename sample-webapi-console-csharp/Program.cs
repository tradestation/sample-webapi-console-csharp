using System;

namespace SymbolSuggestDemo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var api = new TradeStationWebApi(
                Properties.Settings.Default.APIKey,
                Properties.Settings.Default.APISecret,
                Properties.Settings.Default.Environment,
                Properties.Settings.Default.RedirectUri);

            // GetUserAccounts
            var accounts = api.GetUserAccounts();
            foreach (var account in accounts)
            {
                Console.WriteLine(account.Key);
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
