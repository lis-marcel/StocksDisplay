using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ScottPlot;
using ScottPlot.WPF;
using StocksDisplay.Models;

namespace StocksDisplay.View
{
    public partial class DetailedCompanyView : Window
    {
        private readonly List<ScottPlot.OHLC> prices;

        public DetailedCompanyView(List<CompanyData> companyData)
        {
            prices = Services.DataFormatConverter.ConvertToOHLC(companyData).ToList();

            InitializeComponent();
            PrepareWindowItems(companyData);
        }

        private void PrepareWindowItems(List<CompanyData> companyData)
        {
            FillCompanyInformations(companyData);
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

        private void FillCompanyInformations(List<CompanyData> companyData)
        {
            // Display company name
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? fullName)
                ? fullName
                : companyData[0].Ticker;

            CompanyName.Text = $"Data for {companyName}";

            // Load company icon
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            var iconPath = Path.Combine(projectPath, "Media", "Images", $"{companyData[0].Ticker}.png");
            if (File.Exists(iconPath))
            {
                CompanyIcon.Source = new BitmapImage(new Uri(iconPath, UriKind.Absolute));
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
