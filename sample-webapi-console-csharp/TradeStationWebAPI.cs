using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SymbolSuggestDemo
{
    public class TradeStationWebApi
    {
        private string Key { get; set; }
        private string Secret { get; set; }
        private string Host { get; set; }
        private string RedirectUri { get; set; }
        private AccessToken Token { get; set; }

        public TradeStationWebApi(string key, string secret, string environment, string redirecturi)
        {
            this.Key = key;
            this.Secret = secret;
            this.RedirectUri = redirecturi;

            if (environment.Equals("LIVE")) this.Host = "https://api.tradestation.com/v2";
            if (environment.Equals("SIM")) this.Host = "https://sim.api.tradestation.com/v2";

            this.Token = GetAccessToken(GetAuthorizationCode());
        }

        private string GetAuthorizationCode()
        {
            Console.WriteLine("Go here and login:");
            Console.WriteLine(string.Format("{0}/{1}", this.Host,
                                            string.Format(
                                                "authorize?client_id={0}&response_type=code&redirect_uri={1}",
                                                this.Key,
                                                this.RedirectUri)));
            Console.WriteLine("Paste in the authorization code here");

            // Increase Console.ReadLine Input Limit for AuthCode: http://blog.aggregatedintelligence.com/2009/06/consolereadline-and-buffer-size-limits.html
            var inputStream = Console.OpenStandardInput(1024);
            var bytes = new byte[1024];
            var outputLength = inputStream.Read(bytes, 0, 1024);
            var chars = Encoding.UTF8.GetChars(bytes, 0, outputLength);
            return (new string(chars)).Trim();
        }

        private AccessToken GetAccessToken(string authcode)
        {
            Console.WriteLine("Trading the Auth Code for an Access Token...");

            var request = WebRequest.Create(string.Format("{0}/security/authorize", this.Host)) as HttpWebRequest;
            request.Method = "POST";
            var postData =
                string.Format(
                    "grant_type=authorization_code&code={0}&client_id={1}&redirect_uri={2}&client_secret={3}",
                    authcode,
                    this.Key,
                    this.RedirectUri,
                    this.Secret);
            var byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                return GetDeserializedResponse<AccessToken>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }

        private static T GetDeserializedResponse<T>(WebRequest request)
        {
            var response = request.GetResponse() as HttpWebResponse;
            var receiveStream = response.GetResponseStream();
            var readStream = new StreamReader(receiveStream, Encoding.UTF8);
            var ser = new JavaScriptSerializer();
            var json = ser.Deserialize<T>(readStream.ReadToEnd());
            response.Close();
            readStream.Close();
            return json;
        }

        internal IEnumerable<Symbol> SymbolSuggest(string suggestText)
        {
            var resourceUri = new Uri(string.Format("{0}/{1}/{2}?oauth_token={3}", this.Host, "data/symbols/suggest", suggestText, this.Token.access_token));

            Console.WriteLine("Searching for symbols ... ");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                return GetDeserializedResponse<IEnumerable<Symbol>>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }

        }

        internal IEnumerable<AccountInfo> GetUserAccounts()
        {
            var resourceUri =
                new Uri(string.Format("{0}/users/{1}/accounts?oauth_token={2}", this.Host, this.Token.userid,
                                      this.Token.access_token));

            Console.WriteLine("Getting Accounts");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                return GetDeserializedResponse<IEnumerable<AccountInfo>>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }

        internal void GetQuoteChanges(string symbols)
        {
            var resourceUri =
                new Uri(string.Format("{0}/stream/quote/changes/{1}?oauth_token={2}", this.Host, symbols,
                                      this.Token.access_token));

            Console.WriteLine("Streaming Quote/Changes");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8, false, 4096))
                    {
                        var ser = new JavaScriptSerializer();
                        while (true)
                        {
                            var line = readStream.ReadLine();
                            if (line == null) break;
                            var quote = ser.Deserialize<Quote>(line);
                            Console.WriteLine(String.Format("{0}: ASK = {1}; BID = {2}", quote.Symbol, quote.Ask, quote.Bid));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }

        public IEnumerable<OrderDetail> GetOrders(IEnumerable<int> accountKeys)
        {
            var resourceUri =
                new Uri(string.Format("{0}/accounts/{1}/orders?oauth_token={2}", this.Host,
                                      String.Join(",", accountKeys),
                                      this.Token.access_token));

            Console.WriteLine("Getting Orders");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                return GetDeserializedResponse<IEnumerable<OrderDetail>>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }

        public IEnumerable<Quote> GetQuotes(IEnumerable<string> symbols)
        {
            // encode symbols (eg: replace " " with "%20")
            var encodedSymbols = symbols.Select(symbol =>
                {
                    var urlEncode = HttpUtility.UrlEncode(symbol);
                    return urlEncode != null ? urlEncode.Replace("+", "%20") : null;
                });

            var resourceUri =
                new Uri(string.Format("{0}/data/quote/{1}?oauth_token={2}", this.Host,
                                      String.Join(",", encodedSymbols),
                                      this.Token.access_token));

            Console.WriteLine("Getting Quotes");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                return GetDeserializedResponse<IEnumerable<Quote>>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }
    }
}
