using Microsoft.Extensions.Configuration;
using StocksDisplay.Models;
using StocksDisplay.Services;
using StocksDisplay.View;
using StocksDisplay.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StocksDisplay
{
    public partial class MainWindow : Window
    {
        private readonly CompanyStocksService companyStocksService;

        public MainWindow(MainViewModel mainView)
        {
            InitializeComponent();
            DataContext = mainView;

            #region Setup window
            var workingArea = SystemParameters.WorkArea;
            this.Left = workingArea.Right - this.Width - 10; // 10px margin from the right edge
            this.Top = workingArea.Bottom - this.Height - 10; // 10px margin from the bottom edge
            this.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)); // Dark gray color
            #endregion
        }
    }
}
