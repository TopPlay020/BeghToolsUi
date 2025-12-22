using System.Globalization;
using System.Windows.Data;

namespace BeghToolsUi.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value != null;
            if (parameter != null && bool.TryParse(parameter.ToString(), out bool invert) && invert)
                result = !result;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
