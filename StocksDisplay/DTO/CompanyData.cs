using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDisplay.DTO
{
    public class CompanyData
    {
        public string? Ticker { get; set; }
        public DateTime? Date { get; set; }
        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public double? PercentageChange { get; set; }

        public CompanyData(string ticker, DateTime date, double open, double high, double low, double close)
        {
            Ticker = ticker;
            Date = date;
            Open = RoundValue(open);
            High = RoundValue(high);
            Low = RoundValue(low);
            Close = RoundValue(close);
            PercentageChange = CalculatePercentageChange(open, close);
        }

        private static double CalculatePercentageChange(double open, double newest)
        {
            return Math.Round(((newest - open) / open) * 100, 2);
        }

        private static double RoundValue(double value)
        {
            return Math.Round(value, 2);
        }
    }
}
