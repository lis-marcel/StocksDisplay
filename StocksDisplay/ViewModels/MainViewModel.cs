using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StocksDisplay.Models;
using StocksDisplay.Services;
using StocksDisplay.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;

namespace StocksDisplay.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private static readonly List<string> _tickers = ["LMT", /*"BA", "NOC", "TXN", "RTX"*/];
        private readonly CompanyStocksService _companyStocksService;

        [ObservableProperty]
        private readonly ObservableCollection<CompanyData> _companyStocksData;

        public MainViewModel(CompanyStocksService companyStocksService)
        {
            _companyStocksService = companyStocksService;
            _companyStocksData = new();

            LoadData();
        }

        private void LoadData()
        {
            // Fetching company stocks from CSV file for experimental purposes         

            foreach (var ticker in _tickers)
            {
                _companyStocksService.FetchData(ticker);

                if (_companyStocksService.NewestCompanyData != null)
                {
                    CompanyStocksData.Add(_companyStocksService.NewestCompanyData);
                }
            }
        }

        [RelayCommand]
        private void OpenDetails(CompanyData companyData)
        {
            //var detailsWindow = new DetailedCompanyView(companyData);
            //detailsWindow.ShowDialog();
        }
    }
}
