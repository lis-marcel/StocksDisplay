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

            var filteredData = _companyData.GetRange(_companyData.Count - dayMmultiplier, dayMmultiplier);

            var dataX = new DateOnly[filteredData.Count];
            dataX = filteredData.Select(x => x.Date.GetValueOrDefault()).ToArray();

            var dataY = new double[filteredData.Count];
            dataY = filteredData.Select(x => x.Close.Value).ToArray();

            CompanyDataChart.Plot.Add.Scatter(dataX.Select(x => x.DayOfYear).ToArray(), dataY);
            CompanyDataChart.Plot.Axes.AutoScale();

            CompanyDataChart.Refresh();
        }

    }
}
