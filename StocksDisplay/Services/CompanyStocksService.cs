using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Globalization;
using StocksDisplay.Models;

namespace StocksDisplay.Services
{
    public class CompanyStocksService
    {
        private readonly string? localFile;
        private List<CompanyData> companyStocksData;
        private CompanyData newestCompanyData;

        public List<CompanyData> CompanyStocksData
        {
            get => companyStocksData;
        }

        public CompanyData NewestCompanyData
        {
            get => newestCompanyData;
        }

        public CompanyStocksService(IConfiguration configuration)
        {
            localFile = configuration.GetValue<string>("ApiSettings:LocalFile");
        }

        public void FetchData(string companyId)
        {
            companyStocksData = GetCompanyStocks(companyId);
            newestCompanyData = companyStocksData[^1];
        }

        private List<CompanyData> GetCompanyStocks(string companyId)
        {
            var companyStocksData = new List<CompanyData>();

            if (File.Exists(localFile))
            {
                var lines = File.ReadAllLines(localFile).Skip(1);

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data[0].Contains(companyId))
                    {
                        var companyStocks = new CompanyData(
                            companyId,
                            DateOnly.ParseExact(data[2], "yyyyMMdd", CultureInfo.CurrentCulture),
                            double.Parse(data[4], CultureInfo.InvariantCulture),
                            double.Parse(data[5], CultureInfo.InvariantCulture),
                            double.Parse(data[6], CultureInfo.InvariantCulture),
                            double.Parse(data[7], CultureInfo.InvariantCulture));
                        companyStocksData.Add(companyStocks);
                    }
                }
            }

            return companyStocksData;
        }


    }
}
