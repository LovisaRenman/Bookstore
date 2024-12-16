using System.Globalization;
using System.Windows.Data;

namespace BookstoreEf.Converters
{
    class StringToDateOnly : IValueConverter
    {
        // Från databas till textbox
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
            {
                return date.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        // Från textbox databas
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
