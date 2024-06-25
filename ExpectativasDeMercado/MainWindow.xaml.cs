using System.Windows;
using ExpectativasDeMercado.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration; // Importe o namespace System.Configuration
using System.Windows.Controls;


namespace MercadoExpectativaApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var bacenApiService = new BacenApiService();

            // Configuração do DbContext
            var options = new DbContextOptionsBuilder<MarketDbContext>()
                .UseSqlServer(ConfigurationManager.ConnectionStrings["MarketDbConnection"].ConnectionString)
                .Options;

            var dbContext = new MarketDbContext(options);

            // Criando um MainViewModel e configurando DataContext
            var viewModel = new MainViewModel(bacenApiService, dbContext);
            DataContext = viewModel;
        }

    }
}
