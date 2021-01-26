using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Service
{
    public class KeylessCurrecyService : ICurrencyService
    {
        public KeylessCurrecyService(ICurrencyApiSettings settings)
        {
            this.BaseUrl = settings.BaseUrl;
            this.CurrencyListUrl = settings.CurrencyListUrl;
            this.ExchangeRateUrl = settings.ExchangeRateUrl;
        }

        public string BaseUrl { get; private set; }
        public string CurrencyListUrl { get; private set; }
        public string ExchangeRateUrl { get; private set; }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            var jsonCurrencies = await GetStringAsync(this.BaseUrl + this.CurrencyListUrl);
            return ((IEnumerable<KeyValuePair<string, JToken>>)JObject.Parse(jsonCurrencies)).Select(o => new Currency(o.Key, (string)o.Value));
        }

        private static async Task<byte[]> GetBytesAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = await request.GetResponseAsync())
            using (var content = new MemoryStream())
            using (var responseStream = response.GetResponseStream())
            {
                await responseStream.CopyToAsync(content);
                return content.ToArray();
            }
        }

        private static async Task<string> GetStringAsync(string url)
        {
            var bytes = await GetBytesAsync(url);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public async Task<decimal> GetExchangeRate(Currency @base, Currency quote)
        {
            var currencyUrl = string.Format(this.ExchangeRateUrl, @base.Code, quote.Code);
            var rate = await GetStringAsync(this.BaseUrl + currencyUrl);
            return decimal.Parse(JObject.Parse(rate)["rates"][quote.Code].ToString());
        }
    }
}
