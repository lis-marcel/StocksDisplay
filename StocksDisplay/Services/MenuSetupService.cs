using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StocksDisplay.Models;

namespace StocksDisplay.Services
{
    public static class MenuSetupService
    {
        public static void PrepareMenuItems(
            List<CompanyData> companyData,
            TextBlock companyNameTextBlock,
            Image companyLogoImage,
            ComboBox chartOptions,
            string projectPath)
        {
            DisplayCompanyTicker(companyData, companyNameTextBlock, companyLogoImage, projectPath);
            PopulateChartOptions(chartOptions);
        }

        private static void PopulateChartOptions(ComboBox chartOptions)
        {
            foreach (var key in ChartOptions.Options.Keys)
            {
                chartOptions.Items.Add(key);
            }
        }

        private static void DisplayCompanyTicker(
            List<CompanyData> companyData,
            TextBlock companyNameTextBlock,
            Image companyLogoImage,
            string projectPath)
        {
            var ticker = companyData[0].Ticker ?? "";
            var companyName = CompaniesDictionary.Companies.TryGetValue(ticker, out string? value)
                ? value
                : ticker;

            companyNameTextBlock.Text = companyName;

            var imagePath = Path.Combine(projectPath, "Media", "Images", $"{ticker}.png");
            companyLogoImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

    }
}
