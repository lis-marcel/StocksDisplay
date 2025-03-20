using System.Windows;
using System.Windows.Controls;
using ScottPlot;
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

            // Try to get the full company name from the dictionary
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? fullName) ? fullName : companyData[0].Ticker;

            InitialData.Text = $"Initial Data for {companyName}";
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = (ChartOptions.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            foreach (var option in Models.ChartOptions.Options)
            {
                if (option.Key == selectedOption)
                {
                    FilterData(option.Value);
                    break;
                }
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
