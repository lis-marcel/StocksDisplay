using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StocksDisplay.Helpers;
using StocksDisplay.Models;
using StocksDisplay.Services;
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
            projectPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

            InitializeComponent();
            MenuSetupService.PrepareMenuItems(
                companyData,
                CompanyName,
                CompanyLogo,
                ChartOptions,
                projectPath);

            ThemeToggle_Unchecked(null!, null!); // Load initial theme
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            if (ChartOptions.SelectedItem is string selectedOption && Models.ChartOptions.Options.TryGetValue(selectedOption, out int days))
            {
                var filteredData = DataFilter.FilterData(companyData, days);

                var plotModel = ChartBuilderService.CreateCandleStickChart(
                    filteredData,
                    GetOxyColorFromResource("WindowBackgroundBrush"),
                    GetOxyColorFromResource("WindowForegroundBrush"));

                CompanyDataChart.Model = plotModel;
            }
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
