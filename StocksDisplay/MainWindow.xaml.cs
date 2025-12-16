using StocksDisplay.Services;
using StocksDisplay.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace StocksDisplay
{
    public partial class MainWindow : Window
    {
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
