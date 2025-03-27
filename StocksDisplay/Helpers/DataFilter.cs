using StocksDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksDisplay.Helpers
{
    public static class DataFilter
    {
        public static List<CompanyData> FilterData(List<CompanyData> companyData, int dayMultiplier)
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
