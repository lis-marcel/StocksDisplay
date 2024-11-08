using StocksDisplay.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StocksDisplay.DTO;

namespace StocksDisplay
{
    public partial class MainWindow : Window
    {
        private readonly CompanyStocksService _companyStocksService;

        public MainWindow()
        {
            InitializeComponent();
            _companyStocksService = new();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // List of stock tickers to fetch
            var tickers = new List<string> { "LMT", "AAPL", "MSFT" };

            // Fetch company stocks using the service
            var companyStocksList = await _companyStocksService.GetCompanyStocks(tickers);

            // Clear previous data
            StocksStackPanel.Children.Clear();

            // Create and add UI elements for each stock's data
            foreach (var companyStocks in companyStocksList)
            {
                if (companyStocks != null)
                {
                    var symbolTextBlock = new TextBlock { Text = $"Company Symbol: {companyStocks.CompanySymbol}" };
                    var newestTextBlock = new TextBlock { Text = $"Newest: {companyStocks.Newest}$" };
                    var growthTextBlock = new TextBlock { Text = $"Open: {companyStocks.PercentageChange}%" };

                    var stockPanel = new StackPanel();
                    stockPanel.Children.Add(symbolTextBlock);
                    stockPanel.Children.Add(newestTextBlock);
                    stockPanel.Children.Add(growthTextBlock);

                    StocksStackPanel.Children.Add(stockPanel);
                }
            }
        }
    }
}
