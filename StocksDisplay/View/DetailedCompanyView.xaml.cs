using StocksDisplay.DTO;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;
using ScottPlot.WPF;

namespace StocksDisplay.View
{
    /// <summary>
    /// Interaction logic for DetailedCompanyView.xaml
    /// </summary>
    public partial class DetailedCompanyView : Window
    {
        private readonly List<ScottPlot.OHLC> _prices;

        public DetailedCompanyView(List<CompanyData> companyData)
        {
            InitializeComponent();
            _prices = Services.DataFormatConverter.ConvertToOHLC(companyData).ToList();

            // Try to get the full company name from the dictionary
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? fullName) ? fullName : companyData[0].Ticker;

            InitialData.Text = $"Initial Data for {companyName}";
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = (ChartOptions.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (selectedOption)
            {
                case "1 day":
                    FilterData(1);
                    break;
                case "7 days":
                    FilterData(7);
                    break;
                case "1 month":
                    FilterData(30);
                    break;
                case "6 month":
                    FilterData(6 * 30);
                    break;
                case "1 year":
                    FilterData(365);
                    break;
                case "2 years":
                    FilterData(2 * 365);
                    break;
                case "5 years":
                    FilterData(5 * 365);
                    break;
                case "10 years":
                    FilterData(10 * 365);
                    break;
            }
        }

        private void FilterData(int dayMmultiplier)
        {
            CompanyDataChart.Plot.Clear();

            var days = Math.Min(dayMmultiplier, _prices.Count);

            var filteredData = _prices
                .Skip(_prices.Count - days)
                .Take(days)
                .ToList();

            CompanyDataChart.Plot.Add.Candlestick(filteredData);

            CompanyDataChart.Plot.Axes.DateTimeTicksBottom();

            CompanyDataChart.Plot.Axes.AutoScale();

            CompanyDataChart.Refresh();
        }

    }
}
