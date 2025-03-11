using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;
using ScottPlot.WPF;
using StocksDisplay.DTO;

namespace StocksDisplay.Services
{
    internal class DataFormatConverter
    {
        public static IEnumerable<ScottPlot.OHLC> ConvertToOHLC(IEnumerable<CompanyData> data)
        {
            List<ScottPlot.OHLC> values = new();

            foreach (var item in data)
            {
                ScottPlot.OHLC ohlc = new()
                {
                    DateTime = item.Date.GetValueOrDefault().ToDateTime(TimeOnly.MinValue),
                    TimeSpan = TimeSpan.Zero,
                    Close = item.Close.GetValueOrDefault(),
                    High = item.High.GetValueOrDefault(),
                    Low = item.Low.GetValueOrDefault(),
                    Open = item.Open.GetValueOrDefault()
                };

                values.Add(ohlc);
            }

            return values;
        }

    }
}
