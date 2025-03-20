using System.Windows;
using System.Windows.Controls;
using ScottPlot;
using ScottPlot.AxisRules;
using ScottPlot.WPF;
using StocksDisplay.Models;

namespace StocksDisplay.View
{
    /// <summary>
    /// Interaction logic for DetailedCompanyView.xaml
    /// </summary>
    public partial class DetailedCompanyView : Window
    {
        private readonly List<ScottPlot.OHLC> prices;

        public DetailedCompanyView(List<CompanyData> companyData)
        {
            InitializeComponent();
            prices = Services.DataFormatConverter.ConvertToOHLC(companyData).ToList();

            PrepareWindowItems(companyData);
            
        }

        private void PrepareWindowItems(List<CompanyData> companyData)
        {
            // Try to get the full company name from the dictionary
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? fullName) 
                ? fullName : companyData[0].Ticker;

            InitialData.Text = $"Initial Data for {companyName}";

            PopulateChartOptions();

        }

        private void PopulateChartOptions()
        {
            // Populate ComboBox
            ChartOptions.Items.Add("Initial View");
            foreach (var key in Models.ChartOptions.Options.Keys)
            {
                ChartOptions.Items.Add(key);
            }
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = ChartOptions.SelectedItem as string;

            if (Models.ChartOptions.Options.TryGetValue(selectedOption, out int days))
            {
                FilterData(days);
            }

        }

        private void FilterData(int dayMmultiplier)
        {
            CompanyDataChart.Plot.Clear();

            var days = Math.Min(dayMmultiplier, prices.Count);

            var filteredData = prices
                .Skip(prices.Count - days)
                .Take(days)
                .ToList();

            CompanyDataChart.Plot.Add.Candlestick(filteredData);

            CompanyDataChart.Plot.Axes.DateTimeTicksBottom();

            CompanyDataChart.Plot.Axes.AutoScale();

            CompanyDataChart.Refresh();
        }

    }
}
