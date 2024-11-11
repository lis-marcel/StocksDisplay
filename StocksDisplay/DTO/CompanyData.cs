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

        public CompanyData(string symbol, double open, double close)
        {
            CompanySymbol = symbol;
            Open = open;
            Newest = close;
            PercentageChange = CalculatePercentageChange(open, close);
        }

        private static double CalculatePercentageChange(double open, double newest)
        {
            return Math.Round(((newest - open) / open) * 100, 3);
        }
    }
}
