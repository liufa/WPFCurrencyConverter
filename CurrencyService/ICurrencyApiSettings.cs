using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverter.Service
{
    public interface ICurrencyApiSettings
    {
         string BaseUrl { get; set; }
         string CurrencyListUrl { get; set; }
         string ExchangeRateUrl { get; set; }
    }
}
