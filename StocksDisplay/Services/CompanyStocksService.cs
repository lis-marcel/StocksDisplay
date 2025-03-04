using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using StocksDisplay.DTO;
using System.IO;
using System.Globalization;

namespace StocksDisplay.Services
{
    public class CompanyStocksService
    {
        private readonly string? _localFile;

        public CompanyStocksService(IConfiguration configuration)
        {
            _localFile = configuration.GetValue<string>("ApiSettings:LocalFile");
        }

        public async Task<CompanyData> GetCompanyStocks(string companyId)
        {
            var companyStocksData = new List<CompanyData>();
            if (File.Exists(_localFile))
            {
                var lines = File.ReadAllLines(_localFile).Skip(1);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data[0].Contains(companyId))
                    {
                        var companyStocks = new CompanyData(
                            companyId,
                            DateTime.ParseExact(data[2], "yyyyMMdd", CultureInfo.CurrentCulture),
                            double.Parse(data[4], CultureInfo.InvariantCulture),
                            double.Parse(data[5], CultureInfo.InvariantCulture),
                            double.Parse(data[6], CultureInfo.InvariantCulture),
                            double.Parse(data[7], CultureInfo.InvariantCulture));
                        companyStocksData.Add(companyStocks);
                    }
                }
            }
            return companyStocksData.Last();
        }

    }
}
