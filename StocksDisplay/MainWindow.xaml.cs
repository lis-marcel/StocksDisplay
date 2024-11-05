using StocksDisplay.Services;
using System.Collections.Generic;
using System.Windows;
using StocksDisplay.Services;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Fetch company actions using the service
            List<string> companyActions = _companyActionsService.GetCompanyStocks();

            // Display actions in the ListBox
            ActionsListBox.ItemsSource = companyActions;
        }
    }
}
