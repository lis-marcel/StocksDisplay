using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StocksDisplay.Models;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StocksDisplay.View
{
    public partial class DetailedCompanyView : Window
    {
        private readonly List<CompanyData> companyData;
        private readonly string projectPath;

        public DetailedCompanyView(List<CompanyData> companyDataCollection)
        {
            companyData = companyDataCollection;
            projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;

            InitializeComponent();
            PrepareWindowItems();

            ThemeToggle_Unchecked(null, null); // Load initial theme
        }

        private void PrepareWindowItems()
        {
            DisplayCompanyTicker();

            PopulateChartOptions();
        }

        private void PopulateChartOptions()
        {
            foreach (var key in Models.ChartOptions.Options.Keys)
            {
                ChartOptions.Items.Add(key);
            }
        }

        private void DisplayCompanyTicker()
        {
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? value) ?
                        value :
                        companyData[0].Ticker;

            CompanyName.Text = companyName;

            var imagePath = Path.Combine(projectPath!, "Media", "Images", $"{companyData[0].Ticker}.png");

            CompanyLogo.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            if (ChartOptions.SelectedItem is string selectedOption && Models.ChartOptions.Options.TryGetValue(selectedOption, out int days))
            {
                var filteredData = FilterData(days);

                GenerateChart(filteredData);
            }
        }

        private void GenerateChart(List<CompanyData> filteredData)
        {
            var plotModel = new OxyPlot.PlotModel
            {
                Background = GetOxyColorFromResource("WindowBackgroundBrush"),
                TextColor = GetOxyColorFromResource("WindowForegroundBrush"),
                PlotAreaBorderColor = OxyColors.Gray
            };

            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                LabelFormatter = x =>
                {
                    int index = (int)x;
                    if (index >= 0 && index < filteredData.Count)
                    {
                        var date = filteredData[index].Date!.Value.ToDateTime(TimeOnly.MinValue);
                        return date.ToString("yyyy-MM-dd");
                    }
                    return string.Empty;
                },
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                TextColor = plotModel.TextColor,
                TicklineColor = plotModel.TextColor,
                AxislineColor = plotModel.TextColor
            };
            plotModel.Axes.Add(xAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value [$]",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                TextColor = plotModel.TextColor,
                TicklineColor = plotModel.TextColor,
                AxislineColor = plotModel.TextColor
            };
            plotModel.Axes.Add(valueAxis);

            // Create the candlestick series
            var series = new CandleStickSeries
            {
                DataFieldX = "Date",
                DataFieldHigh = "High",
                DataFieldLow = "Low",
                DataFieldOpen = "Open",
                DataFieldClose = "Close",
                IncreasingColor = OxyColor.Parse("#2eba7c"),
                DecreasingColor = OxyColor.Parse("#f93647"),
                CandleWidth = 0.6
            };

            // Assign X-values as indices
            int dataIndex = 0;
            foreach (var data in filteredData)
            {
                series.Items.Add(new HighLowItem
                {
                    X = dataIndex++,
                    High = data.High!.Value,
                    Low = data.Low!.Value,
                    Open = data.Open!.Value,
                    Close = data.Close!.Value,
                });
            }
            plotModel.Series.Add(series);

            CompanyDataChart.Model = plotModel;
        }

        private List<CompanyData> FilterData(int dayMultiplier)
        {
            var days = Math.Min(dayMultiplier, companyData.Count);

            var filteredData = companyData
                .Where(d => d.Date.HasValue && d.Open.HasValue && d.High.HasValue && d.Low.HasValue && d.Close.HasValue)
                .Skip(companyData.Count - days)
                .Take(days)
                .ToList();

            return filteredData;
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            var darkTheme = new ResourceDictionary
            {
                Source = new Uri($"{projectPath}/View/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(darkTheme);
            RefreshChartColors();

            ThemeIcon.Source = new BitmapImage(new Uri(Path.Combine(projectPath, "Media", "Icons", "moon.png")));
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            var lightTheme = new ResourceDictionary
            {
                Source = new Uri($"{projectPath}/View/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(lightTheme);
            RefreshChartColors();

            ThemeIcon.Source = new BitmapImage(new Uri(Path.Combine(projectPath, "Media", "Icons", "sun.png")));
        }

        private void RefreshChartColors()
        {
            if (CompanyDataChart.Model == null) return;

            var plotModel = CompanyDataChart.Model;
            var newBackground = GetOxyColorFromResource("WindowBackgroundBrush");
            var newForeground = GetOxyColorFromResource("WindowForegroundBrush");

            // Match main chart background
            plotModel.Background = newBackground;
            plotModel.PlotAreaBackground = newBackground;
            plotModel.TextColor = newForeground;
            plotModel.PlotAreaBorderColor = newForeground;

            // Update axes
            foreach (var axis in plotModel.Axes)
            {
                axis.TextColor = newForeground;
                axis.TicklineColor = newForeground;
                axis.AxislineColor = newForeground;
            }

            // Redraw the chart
            CompanyDataChart.InvalidatePlot(true);
        }

        private OxyColor GetOxyColorFromResource(string resourceKey)
        {
            if (Application.Current.TryFindResource(resourceKey) is System.Windows.Media.SolidColorBrush brush)
            {
                return OxyColor.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
            }
            return OxyColors.Automatic;
        }

        private void CenterView_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyDataChart.Model != null)
            {
                foreach (var axis in CompanyDataChart.Model.Axes)
                {
                    axis.Reset();
                }
                CompanyDataChart.InvalidatePlot(true);
            }
        }

    }
}
