using CurrencyConverter.Service;
using System.Windows;

namespace WpfCurencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CurrencyConverterViewModel converterViewModel;

        public MainWindow(ICurrencyService currencyService, IWritableOptions<UserSettings> userSettings)
        {
            InitializeComponent();
            converterViewModel = new CurrencyConverterViewModel(currencyService, userSettings);
            DataContext = converterViewModel;
        }

        public MainWindow()
        {
        }
    }
}
