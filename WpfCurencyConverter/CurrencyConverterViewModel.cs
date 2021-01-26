using CurrencyConverter.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WpfCurencyConverter
{
    public class CurrencyConverterViewModel : INotifyPropertyChanged
    {
        private ICurrencyService CurrencyService { get; }
        public IWritableOptions<UserSettings> UserSettings { get; }

        public bool IsInitalized { get; private set; }

        private ObservableCollection<Currency> _base;
        private ObservableCollection<Currency> quote;
        private Currency selectedBaseCurrency;
        private Currency selectedQuoteCurrency;
        private string rate;
        public CurrencyConverterViewModel(ICurrencyService currencyService,IWritableOptions<UserSettings> userSettings)
        {
            CurrencyService = currencyService;
            UserSettings = userSettings;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Initialize();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public CurrencyConverterViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected async void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
            if (info != "Rate")
            {
                await CurrencyChanged();
            }
        }

        private async Task Initialize()
        {
            await GetCurrencies().ContinueWith(c =>
                {
                   var currencies = c.Result;
                    this.Base = new ObservableCollection<Currency>(currencies);
                    this.Quote = new ObservableCollection<Currency>(currencies);
                    SetUserSettings(currencies);
                    IsInitalized = true;
                });
        }

        private void SetUserSettings(IEnumerable<Currency> currencies) 
        {
            if (this.UserSettings.Value != null) {
                var settings = this.UserSettings.Value;
                this.SelectedBaseCurrency = currencies.SingleOrDefault(o => o.Code == settings.Base);
                this.SelectedQuoteCurrency = currencies.SingleOrDefault(o => o.Code == settings.Quote);
            }
        }

        private async Task<IEnumerable<Currency>> GetCurrencies()
        {
            return await this.CurrencyService.GetCurrencies();
        }

        public Currency SelectedBaseCurrency 
        {
            get => selectedBaseCurrency; 
            set 
            { 
                selectedBaseCurrency = value;
                NotifyPropertyChanged(nameof(SelectedBaseCurrency));
            }
        }

        public Currency SelectedQuoteCurrency 
        { 
            get => selectedQuoteCurrency; 
            set 
            {
                selectedQuoteCurrency = value; 
                NotifyPropertyChanged(nameof(SelectedQuoteCurrency));
            }
        }

        public ObservableCollection<Currency> Base
        {
            get
            {
                return _base;
            }
            set
            {
                _base = value;
                NotifyPropertyChanged("Base");

            }
        }

        public ObservableCollection<Currency> Quote 
        { 
            get => quote; 
            set 
            {
                quote = value; 
                NotifyPropertyChanged(nameof(Quote));
            }
        }

        public string Rate
        {
            get => rate;
            set
            {
                rate = value;
                NotifyPropertyChanged(nameof(Rate));
            }
        }

        private async Task CurrencyChanged()
        {
            SaveUserOptions();
            if (SelectedBaseCurrency != null && SelectedQuoteCurrency != null)
            {
                if (SelectedBaseCurrency.Code == SelectedQuoteCurrency.Code)
                {
                    Rate = "1.0000";
                }
                else
                {
                    var rate = await this.CurrencyService.GetExchangeRate(SelectedBaseCurrency, SelectedQuoteCurrency);
                   this.Rate = $"{rate:0.0000}";
                }
            }
        }

        private void SaveUserOptions() 
        {
            UserSettings.Update(opt => {
                opt.Base = SelectedBaseCurrency?.Code;
                opt.Quote = SelectedQuoteCurrency?.Code;
            });
        }
    }
}
