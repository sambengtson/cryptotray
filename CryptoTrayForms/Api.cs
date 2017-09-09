using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Runtime.Serialization;
using System.Threading;
using Newtonsoft.Json;

namespace CryptoTrayForms
{
    public class Api
    {
        RestClient client;

        public Api()
        {
            client = new RestClient("https://api.cryptonator.com/api/ticker");
        }

        public List<CryptResponse> GetMajorCryptoPrices()
        {
            var btc = GetBTC();
            var ltc = GetLTC();
            var eth = GetETH();

            var prices = new List<CryptResponse>();
            prices.Add(btc.Result);
            prices.Add(ltc.Result);
            prices.Add(eth.Result);

            return prices;
        }

        private async Task<CryptResponse> GetBTC()
        {
            var req = new RestRequest("/btc-usd", Method.GET);
            
            var response = await client.ExecuteTaskAsync(req);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Invalid status code");
            }

            return JsonConvert.DeserializeObject<CryptResponse>(response.Content);
        }

        private async Task<CryptResponse> GetLTC()
        {
            var req = new RestRequest("/ltc-usd", Method.GET);

            var response = await client.ExecuteTaskAsync(req);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Invalid status code");
            }

            return JsonConvert.DeserializeObject<CryptResponse>(response.Content);
        }

        private async Task<CryptResponse> GetETH()
        {
            var req = new RestRequest("/eth-usd", Method.GET);

            var response = await client.ExecuteTaskAsync(req);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Invalid status code");
            }

            return JsonConvert.DeserializeObject<CryptResponse>(response.Content);
        }
    }

    public class CryptResponse
    {
        [JsonProperty(PropertyName = "ticker")]
        public Ticker Response { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool IsSuccess { get; set; }
    }
    
    public class Ticker
    {
        [JsonProperty(PropertyName = "base")]
        public string Base { get; set; }

        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public decimal Volume { get; set; }

        [JsonProperty(PropertyName = "change")]
        public decimal Change { get; set; }
    }
}
