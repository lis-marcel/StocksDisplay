using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using StocksDisplay.DTO;
using System.IO;

namespace StocksDisplay.Services
{
    public class CompanyStocksService
    {
        private readonly string _apiKey = "AGqIVMhfoTWgh2r1WHcdbIcR35XBCkPE";
        private readonly string _apiUrl = "https://api.polygon.io/v2/aggs/ticker/";

        public async Task<List<CompanyData>> GetCompanyStocks(List<string> tickers)
        {
            var tasks = tickers.Select(ticker => FetchCompanyStocksFromAPI(ticker)).ToList();
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        private async Task<CompanyData> FetchCompanyStocksFromAPI(string ticker)
        {
            var sb = new StringBuilder();

            sb.Append(_apiUrl +
                ticker +
                "/range/1/hour/2024-11-06/2024-11-06?adjusted=true&sort=asc&apiKey=" +
                _apiKey);

            var apiResponse = await new HttpClient().GetAsync(sb.ToString());

            var stringResponse = await apiResponse.Content.ReadAsStringAsync();

            var companyStocksData = ExtractStocksData(stringResponse);

            return companyStocksData;
        }

        private static CompanyData ExtractStocksData(string stringResponse)
        {
            try
            {
                // Check if the response is a valid JSON object
                if (stringResponse.Trim().StartsWith("{"))
                {
                    var json = JObject.Parse(stringResponse);

                    // Extract Company data
                    var company = json["ticker"]?.ToString();
                    var open = double.Parse(json["results"][0]["o"]?.ToString());
                    var newest = double.Parse(json["results"].Last["c"]?.ToString());

                    var companyStocks = new CompanyData(company, open, newest);

                    return companyStocks;
                }
                else
                {
                    throw new Exception("Invalid JSON response format.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and the response content
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                Console.WriteLine($"Response content: {stringResponse}");
                throw;
            }
        }

    }
}
