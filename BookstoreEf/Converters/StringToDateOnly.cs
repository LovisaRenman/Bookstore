using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookstoreEf.Converters
{
    class StringToDateOnly : IValueConverter
    {
        // Konvertera data från en källa (ex. databas) till en target (ex. textbox)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
            {
                return date.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        // Konvertera data från en target (ex. textbox) till en källa (ex. databas)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateStr)
            {
                return DateOnly.Parse(dateStr);
            }
            return null;
        }
    }
}
