namespace StocksDisplay.Services
{
    public class CompanyStocksService
    {
        public List<string> GetCompanyStocks()
        {
            // This method should ideally fetch data from a database or an API
            // For now, we'll return a static list of actions
            return new List<string>
            {
                "Company A: Acquired Company B",
                "Company C: Launched new product",
                "Company D: Announced quarterly earnings"
            };
        }
    }
}
