using CurrencyConverter.Service;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.Service.Tests
{
    public class Tests
    {
        public KeylessCurrecyService KeylessCurrecyService { get; private set; }

        [SetUp]
        public void Setup()
        {
            KeylessCurrecyService = new KeylessCurrecyService(new CurrencyApiSettings
            {
                BaseUrl = "https://api.frankfurter.app/",
                CurrencyListUrl = "currencies",
                ExchangeRateUrl = "latest?amount=1&from={0}&to={1}"
            });
        }

        [Test]
        [TestCaseSource("GetCurrencyListTestCaseData")]
        public void GetCurrencyList(List<Currency> expected)
        {
            var actual = KeylessCurrecyService.GetCurrencies().Result.ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.IsTrue(expected[i].Equals(actual[i]));
            }
        }

        private static IEnumerable<TestCaseData> GetCurrencyListTestCaseData()
        {
            yield return new TestCaseData(new List<Currency> {
                                    new Currency("AUD", "Australian Dollar"),
                                    new Currency("BGN", "Bulgarian Lev"),
                                    new Currency("BRL", "Brazilian Real"),
                                    new Currency("CAD", "Canadian Dollar"),
                                    new Currency("CHF", "Swiss Franc"),
                                    new Currency("CNY", "Chinese Renminbi Yuan"),
                                    new Currency("CZK", "Czech Koruna"),
                                    new Currency("DKK", "Danish Krone"),
                                    new Currency("EUR", "Euro"),
                                    new Currency("GBP", "British Pound"),
                                    new Currency("HKD", "Hong Kong Dollar"),
                                    new Currency("HRK", "Croatian Kuna"),
                                    new Currency("HUF", "Hungarian Forint"),
                                    new Currency("IDR", "Indonesian Rupiah"),
                                    new Currency("ILS", "Israeli New Sheqel"),
                                    new Currency("INR", "Indian Rupee"),
                                    new Currency("ISK", "Icelandic Króna"),
                                    new Currency("JPY", "Japanese Yen"),
                                    new Currency("KRW", "South Korean Won"),
                                    new Currency("MXN", "Mexican Peso"),
                                    new Currency("MYR", "Malaysian Ringgit"),
                                    new Currency("NOK", "Norwegian Krone"),
                                    new Currency("NZD", "New Zealand Dollar"),
                                    new Currency("PHP", "Philippine Peso"),
                                    new Currency("PLN", "Polish Złoty"),
                                    new Currency("RON", "Romanian Leu"),
                                    new Currency("RUB", "Russian Ruble"),
                                    new Currency("SEK", "Swedish Krona"),
                                    new Currency("SGD", "Singapore Dollar"),
                                    new Currency("THB", "Thai Baht"),
                                    new Currency("TRY", "Turkish Lira"),
                                    new Currency("USD", "United States Dollar"),
                                    new Currency("ZAR","South African Rand") });
        }

        [Test]
        public void GetExchangeRate()
        {
            var currencies = KeylessCurrecyService.GetCurrencies().Result.ToList();
            var actual = KeylessCurrecyService.GetExchangeRate(currencies[0], currencies[1]).Result;
            Assert.IsTrue(actual != 0m);
        }
    }
}