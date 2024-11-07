using StocksDisplay.Services;
using System.Collections.Generic;
using System.Windows;
using StocksDisplay.Services;
using StocksDisplay.DTO;

namespace StocksDisplay
{
    public partial class MainWindow : Window
    {
        private readonly CompanyStocksService _companyActionsService;

        public MainWindow()
        {
            InitializeComponent();
            _companyActionsService = new();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Fetch company actions using the service
            var companyStocks = await _companyActionsService.GetCompanyStocks();

            if (companyStocks != null)
            {
                // Display the data in the UI
                SymbolTextBlock.Text = $"CompanySymbol: {companyStocks.CompanySymbol}";
                NewestTextBlock.Text = $"Newest: {companyStocks.Newest}$";
                GrowthTextBlock.Text = $"Open: {companyStocks.PercentageChange}%";
            }
        }
    }
}
