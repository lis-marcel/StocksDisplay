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
        private readonly List<CompanyData> _companyData;

        public DetailedCompanyView(List<CompanyData> companyData)
        {
            InitializeComponent();
            _companyData = companyData;

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

            var days = Math.Min(dayMmultiplier, _companyData.Count);

            var filteredData = _companyData
                .Skip(_companyData.Count - days)
                .Take(days)
                .ToList();

            var dataX = filteredData
                .Select(x => x.Date.GetValueOrDefault().ToDateTime(TimeOnly.MinValue))
                .ToArray();

            var dataY = filteredData
                .Select(x => x.Close.GetValueOrDefault())
                .ToArray();

            double[] dataX_AODate = dataX.Select(x => x.ToOADate()).ToArray();

            CompanyDataChart.Plot.Add.Scatter(dataX_AODate, dataY);

            CompanyDataChart.Plot.Axes.DateTimeTicksBottom();

            CompanyDataChart.Plot.Axes.AutoScale();

            CompanyDataChart.Refresh();
        }

    }
}
