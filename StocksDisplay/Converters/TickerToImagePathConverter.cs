using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StocksDisplay.Converters
{
    public class TickerToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string ticker && !string.IsNullOrEmpty(ticker))
            {
                try
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    var projectPath = Directory.GetParent(currentDir)?.Parent?.Parent?.FullName;

                    if (projectPath != null)
                    {
                        var imagePath = Path.Combine(projectPath, "Media", "Images", $"{ticker}.png");

                        if (File.Exists(imagePath))
                        {
                            // Tworzymy BitmapImage z pliku
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad; // Ważne, żeby nie blokować pliku
                            bitmap.EndInit();
                            return bitmap;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}