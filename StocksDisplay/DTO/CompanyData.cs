using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDisplay.DTO
{
    public class CompanyData
    {
        public string? CompanySymbol { get; set; }
        public double? Open { get; set; }
        public double? Newest { get; set; }
        public double? PercentageChange { get; set; }

        public CompanyData(string symbol, double open, double newest)
        {
            CompanySymbol = symbol;
            Open = RoundValue(open);
            Newest = RoundValue(newest);
            PercentageChange = CalculatePercentageChange(open, newest);
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
