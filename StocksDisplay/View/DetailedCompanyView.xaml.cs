using StocksDisplay.DTO;
using System.Windows;
using System.Windows.Controls;

namespace StocksDisplay.MVM
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
            switch (selectedOption) {
                case "1 day":
                    FilterData(1);
                    break;
                case "7 days":
                    FilterData(2);
                    break;
                case "1 month":
                    break;
                case "6 month":
                    break;
                case "1 year":
                    break;
                case "2 years":
                    break;
                case "5 years":
                    break;
                case "10 years":
                    break;
            }
        }
                
        private void FilterData(int dayMmultiplier)
        {
            // Filter data based on the selected option

            var filteredData = _companyData.GetRange(_companyData.Count - dayMmultiplier, dayMmultiplier);
        }

    }
}
