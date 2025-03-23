using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StocksDisplay.Models;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace StocksDisplay.View
{
    public partial class DetailedCompanyView : Window
    {
        private readonly List<CompanyData> companyData;

        public DetailedCompanyView(List<CompanyData> companyDataCollection)
        {
            companyData = companyDataCollection;

            InitializeComponent();

            PrepareWindowItems();
        }

        private void PrepareWindowItems()
        {
            DisplayCompanyTicker();

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

        private void DisplayCompanyTicker()
        {
            var companyName = CompaniesDictionary.Companies.TryGetValue(companyData[0].Ticker, out string? value) ?
                        value :
                        companyData[0].Ticker;

            CompanyName.Text = companyName;

            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            var imagePath = Path.Combine(projectPath!, "Media", "Images", $"{companyData[0].Ticker}.png");

            CompanyLogo.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        private void ShowChart_Click(object sender, RoutedEventArgs e)
        {
            var selectedOption = ChartOptions.SelectedItem as string;

            if (Models.ChartOptions.Options.TryGetValue(selectedOption, out int days))
            {
                var filteredData = FilterData(days);

                GenerateChart(filteredData);
            }
        }

        private void GenerateChart(List<CompanyData> filteredData)
        {
            var plotModel = new PlotModel();

            // Replace DateTimeAxis with LinearAxis and use index-based X values
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                LabelFormatter = x =>
                {
                    int index = (int)x;
                    if (index >= 0 && index < filteredData.Count)
                    {
                        var date = filteredData[index].Date.Value.ToDateTime(TimeOnly.MinValue);
                        return date.ToString("yyyy-MM-dd");
                    }
                    return string.Empty;
                },

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColors.LightGray,
                MinorGridlineColor = OxyColors.LightGray
            };
            plotModel.Axes.Add(xAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value [$]",

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineColor = OxyColors.LightGray,
                MinorGridlineColor = OxyColors.LightGray
            };
            plotModel.Axes.Add(valueAxis);

            var series = new CandleStickSeries
            {
                Title = "Company Data",
                DataFieldX = "Date",
                DataFieldHigh = "High",
                DataFieldLow = "Low",
                DataFieldOpen = "Open",
                DataFieldClose = "Close",

                IncreasingColor = OxyColors.Green,
                DecreasingColor = OxyColors.Red,
                CandleWidth = 0.6,
            };

            // Assign index-based X values to each data point
            int dataIndex = 0;
            foreach (var data in filteredData)
            {
                series.Items.Add(new HighLowItem
                {
                    X = dataIndex++,
                    High = data.High.Value,
                    Low = data.Low.Value,
                    Open = data.Open.Value,
                    Close = data.Close.Value
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

    }
}
