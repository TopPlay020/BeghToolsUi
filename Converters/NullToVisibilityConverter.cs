using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BeghToolsUi.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null && bool.TryParse(parameter.ToString(), out bool parsed))
                invert = parsed;

            bool isNullOrEmpty = string.IsNullOrEmpty(value as string);
            if (invert)
                isNullOrEmpty = !isNullOrEmpty;

            return isNullOrEmpty ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
