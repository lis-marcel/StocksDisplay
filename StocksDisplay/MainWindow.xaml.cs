using StocksDisplay.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StocksDisplay.DTO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Media;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using StocksDisplay.MVM;

namespace StocksDisplay
{
    public partial class MainWindow : Window
    {
        private readonly CompanyStocksService _companyStocksService;

        public static readonly List<string> Tickers = ["LMT", /*"BA", "NOC", "TXN", "RTX"*/];

        public MainWindow(IConfiguration configuration)
        {
            InitializeComponent();
            _companyStocksService = new CompanyStocksService(configuration);

            #region Setup window
            var workingArea = SystemParameters.WorkArea;
            this.Left = workingArea.Right - this.Width - 10; // 10px margin from the right edge
            this.Top = workingArea.Bottom - this.Height - 10; // 10px margin from the bottom edge
            this.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)); // Dark gray color
            #endregion

            LoadData(this, new RoutedEventArgs());
        }

        private async void LoadData(object sender, RoutedEventArgs e)
        {
            // Fetching company stocks from CSV file for experimental purposes         
            _companyStocksService.FetchData(Tickers[0]);

            // Clear previous data
            StocksStackPanel.Children.Clear();

            if (_companyStocksService.NewestCompanyData != null)
            {
                var stockPanel = CreateStackPanel(_companyStocksService.NewestCompanyData);

                if (stockPanel.Parent != null)
                {
                    var parent = stockPanel.Parent as Panel;
                    parent?.Children.Remove(stockPanel);
                }

                StocksStackPanel.Children.Add(stockPanel);
            }
        }

        private StackPanel CreateStackPanel(CompanyData companyData)
        {
            var stockPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 1),
            };

            stockPanel.MouseLeftButtonDown += (sender, e) => OpenDetailsWindow(sender, e, _companyStocksService.CompanyStocksData);

            StyleStackPanel(stockPanel, companyData);         

            return stockPanel;
        }

        private static StackPanel StyleStackPanel(StackPanel stockPanel, CompanyData companyData)
        {
            // Determine the gradient based on the company's growth
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };

            if (companyData.PercentageChange.HasValue && companyData.PercentageChange.Value > 0)
            {
                gradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.0));
                gradientBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 1.0));
            }
            else
            {
                gradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                gradientBrush.GradientStops.Add(new GradientStop(Colors.LightCoral, 1.0));
            }

            stockPanel.Background = gradientBrush;

            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var logo = new Image
            {
                Source = new BitmapImage(new Uri($"{projectPath}\\Media\\Images\\{companyData.Ticker}.png", UriKind.RelativeOrAbsolute)),
                Width = 50,
                Height = 50,
                Margin = new Thickness(0, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            var textPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData.Ticker, out string? value) ?
                        value :
                        companyData.Ticker;
            var symbolTextBlock = new TextBlock { Text = $"{companyName}", FontWeight = FontWeights.Bold };
            var newestTextBlock = new TextBlock { Text = $"Current: {companyData.Close}$" };
            var growthTextBlock = new TextBlock { Text = $"Growth: {companyData.PercentageChange}%" };

            textPanel.Children.Add(symbolTextBlock);
            textPanel.Children.Add(newestTextBlock);
            textPanel.Children.Add(growthTextBlock);

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) }); // Fixed width for logo
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Remaining space for text

            Grid.SetColumn(logo, 0);
            Grid.SetColumn(textPanel, 1);

            grid.Children.Add(logo);
            grid.Children.Add(textPanel);

            stockPanel.Children.Add(grid);

            return stockPanel;
        }

        private static void OpenDetailsWindow(object sender, RoutedEventArgs e, List<CompanyData> companyData)
        {
            var detailsWindow = new DetailedCompanyView(companyData);
            detailsWindow.Show();
        }

    }
}
