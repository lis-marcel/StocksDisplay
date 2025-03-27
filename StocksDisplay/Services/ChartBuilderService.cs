using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StocksDisplay.Models;

namespace StocksDisplay.Services
{
    public static class ChartBuilderService
    {
        public static PlotModel CreateCandleStickChart(
            List<CompanyData> filteredData,
            OxyColor background,
            OxyColor foreground)
        {
            var plotModel = new PlotModel
            {
                Background = background,
                TextColor = foreground,
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
                TextColor = foreground,
                TicklineColor = foreground,
                AxislineColor = foreground
            };
            plotModel.Axes.Add(xAxis);

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value [$]",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                TextColor = foreground,
                TicklineColor = foreground,
                AxislineColor = foreground
            };
            plotModel.Axes.Add(yAxis);

            var series = new CandleStickSeries
            {
                IncreasingColor = OxyColor.Parse("#2eba7c"),
                DecreasingColor = OxyColor.Parse("#f93647"),
                CandleWidth = 0.6
            };

            int dataIndex = 0;
            foreach (var data in filteredData)
            {
                series.Items.Add(new HighLowItem
                {
                    X = dataIndex++,
                    High = data.High!.Value,
                    Low = data.Low!.Value,
                    Open = data.Open!.Value,
                    Close = data.Close!.Value
                });
            }

            plotModel.Series.Add(series);
            return plotModel;
        }
    }
}
