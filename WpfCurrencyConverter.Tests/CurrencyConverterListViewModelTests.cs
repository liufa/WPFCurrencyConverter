using CurrencyConverter.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfCurencyConverter;

namespace WpfCurrencyConverter.Tests
{
    public class CurrencyConverterCheckListViewModelTests
    {

        public CurrencyConverterViewModel CurrencyConverterViewModel { get; private set; }

        [SetUp]
        public void Setup()
        {
            var service = new Mock<ICurrencyService>();
            service.Setup(o => o.GetCurrencies()).ReturnsAsync((IEnumerable<Currency>)new List<Currency> {
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
                                    new Currency("ZAR","South African Rand") }
           );
            service.Setup(o => o.GetExchangeRate(
                It.Is<Currency>(o => o.Equals(new Currency("GBP", "British Pound"))),
                It.Is<Currency>(o => o.Equals(new Currency("AUD", "Australian Dollar"))))).ReturnsAsync(0.1234M);

            var appSettingsWriter = new Mock<IWritableOptions<UserSettings>>();
            appSettingsWriter.Setup(o => o.Value).Returns(new UserSettings { Base = "EUR", Quote = "USD" });


            this.CurrencyConverterViewModel = new CurrencyConverterViewModel(service.Object, appSettingsWriter.Object);
            while (!this.CurrencyConverterViewModel.IsInitalized)
            {
                Task.Delay(100);
            }
        }


        [Test]
        public async Task SetUserSettings()
        {
            await Task.Run(() =>
            {
                Assert.IsTrue(this.CurrencyConverterViewModel.SelectedBaseCurrency.Code == "EUR");
                Assert.IsTrue(this.CurrencyConverterViewModel.SelectedQuoteCurrency.Code == "USD");
            });

        }
    }
}