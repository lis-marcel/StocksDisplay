using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDisplay.Models
{
    public class ChartOptions
    {
        public static readonly Dictionary<string, int> Options =
            new()
            {
                ["7 days"] = 7,
                ["1 month"] = 30,
                ["6 month"] =  6 * 30,
                ["1 year"] = 365,
                ["2 years"] = 2 * 365,
                ["5 years"] = 5 * 365,
                ["10 years"] = 10 * 365
            };

    }
}
