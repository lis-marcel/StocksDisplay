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
        private readonly CompanyData _companyData;

        public DetailedCompanyView(CompanyData companyData)
        {
            InitializeComponent();
            _companyData = companyData;

            // Try to get the full company name from the dictionary
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData.CompanySymbol, out string? fullName) ? fullName : companyData.CompanySymbol;

            InitialData.Text = $"Company: {companyName}\n" +
                               $"Newest: {companyData.Newest}\n" +
                               $"Percentage Change: {companyData.PercentageChange}%";
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = (ChartOptions.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedOption != null)
            {
                // Logic to display the chart based on the selected option
                MessageBox.Show($"Selected Chart Option: {selectedOption}");
            }
        }
    }
}
