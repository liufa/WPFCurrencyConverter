namespace CurrencyConverter.Service
{
    public class CurrencyApiSettings : ICurrencyApiSettings
    {
        public string BaseUrl { get; set; }
        public string Key { get; set; }
        public string CurrencyListUrl { get; set; }
        public string ExchangeRateUrl { get; set; }
    }
}
