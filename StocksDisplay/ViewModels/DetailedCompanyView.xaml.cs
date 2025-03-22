using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StocksDisplay.Models;

namespace StocksDisplay.View
{
    public partial class DetailedCompanyView : Window
    {
        private readonly List<CompanyData> companyData;

        public DetailedCompanyView(List<CompanyData> companyData)
        {
            this.companyData = companyData;

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

        private IEnumerable<CompanyData> FilterData(int dayMultiplier)
        {
            var days = Math.Min(dayMultiplier, companyData.Count);

            var filteredData = companyData
                .Where(d => d.Date.HasValue && d.Open.HasValue && d.High.HasValue && d.Low.HasValue && d.Close.HasValue)
                .Skip(companyData.Count - days)
                .Take(days)
                .ToList();

            return filteredData;
        }

        private void GenerateChart(IEnumerable<CompanyData> filteredData)
        {
            var plotModel = new PlotModel();

            var filteredDataList = filteredData.ToList();

            // Replace DateTimeAxis with LinearAxis and use index-based X values
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                LabelFormatter = x =>
                {
                    int index = (int)x;
                    if (index >= 0 && index < filteredDataList.Count)
                    {
                        var date = filteredDataList[index].Date.Value.ToDateTime(TimeOnly.MinValue);
                        return date.ToString("yyyy-MM-dd");
                    }
                    return string.Empty;
                }
            };
            plotModel.Axes.Add(xAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value [$]"
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
                CandleWidth = 0.6
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


    }
}
