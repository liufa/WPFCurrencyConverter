using CurrencyConverter.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace WpfCurencyConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ICurrencyService, KeylessCurrecyService>(o => new KeylessCurrecyService(Configuration.GetSection("AppSettings:CurrencyApiSettings").Get<CurrencyApiSettings>()));
            serviceCollection.AddTransient(typeof(MainWindow));
            serviceCollection.ConfigureWritable<UserSettings>(Configuration.GetSection("UserSettings"));
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
