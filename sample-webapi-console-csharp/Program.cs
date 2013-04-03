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

            Console.Write("Enter symbol: ");
            var suggestText = Console.ReadLine();
            var symbols = api.SymbolSuggest(suggestText);
            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach (var symbol in symbols)
            {
                Console.WriteLine(symbol.Name);
            }
            Console.ReadLine();
        }
    }
}
