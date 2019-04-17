﻿using System;
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
            
            // Disable Tls 1.0 and use Tls 1.2
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            // these two lines are only needed if .net 4.5 is not installed
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;

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

            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(this.RedirectUri);
                listener.Start();
                Console.WriteLine("\nEmbedded HTTP Server is Listening for Authorization Code...");

                var context = listener.GetContext();
                var req = context.Request;
                var res = context.Response;

                var responseString = "<html><body><script>window.open('','_self').close();</script></body></html>";
                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                res.ContentLength64 = buffer.Length;
                var output = res.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                listener.Stop();
                return req.QueryString.Get("code");
            }
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
            var json = readStream.ReadToEnd();
            var scrubbedJson =
                json.Replace(
                    "\"__type\":\"EquitiesOptionsOrderConfirmation:#TradeStation.Web.Services.DataContracts\",", ""); // hack
            var deserializaed = ser.Deserialize<T>(scrubbedJson);
            response.Close();
            readStream.Close();
            return deserializaed;
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

        public IEnumerable<Confirmation> GetConfirmations(Order order)
        {
            var serializer = new JavaScriptSerializer();
            var orderjson = serializer.Serialize(order);

            var resourceUri =
                new Uri(string.Format("{0}/orders/confirm?oauth_token={1}", this.Host, this.Token.access_token));

            Console.WriteLine("Getting Order Confirmation");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "POST";
            var postData = orderjson;
            var byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                return GetDeserializedResponse<IEnumerable<Confirmation>>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                Environment.Exit(-1);
                throw;
            }
        }

        public IEnumerable<OrderResult> PlaceOrder(Order order)
        {
            var serializer = new JavaScriptSerializer();
            var orderjson = serializer.Serialize(order);

            var resourceUri =
                new Uri(string.Format("{0}/orders?oauth_token={1}", this.Host, this.Token.access_token));

            Console.WriteLine("Placing Order");

            var request = WebRequest.Create(resourceUri) as HttpWebRequest;
            request.Method = "POST";
            var postData = orderjson;
            var byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                return GetDeserializedResponse<IEnumerable<OrderResult>>(request);
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
