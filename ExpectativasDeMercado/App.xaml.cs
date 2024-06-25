using ExpectativasDeMercado.Data;
using MercadoExpectativaApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Windows;

namespace ExpectativasDeMercado
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ConfigureServices();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configuração do DbContext
            services.AddDbContext<MarketDbContext>(options =>
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["MarketDbConnection"].ConnectionString));

            // Registrando serviços necessários
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<BacenApiService>();

            // Construindo o provedor de serviços
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
