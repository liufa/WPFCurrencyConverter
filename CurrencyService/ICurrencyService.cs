using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Service
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrencies();

        Task<decimal> GetExchangeRate(Currency @base, Currency quote);
    }
}
