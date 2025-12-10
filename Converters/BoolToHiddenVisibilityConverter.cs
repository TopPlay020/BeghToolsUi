using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BeghToolsUi.Converters
{
    public class BoolToHiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null && bool.TryParse(parameter.ToString(), out bool parsed))
                invert = parsed;

            if (value is bool boolValue)
            {
                if (invert)
                    boolValue = !boolValue;
                return boolValue ? Visibility.Visible : Visibility.Hidden;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null && bool.TryParse(parameter.ToString(), out bool parsed))
                invert = parsed;

            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                return invert ? !result : result;
            }
            return false;
        }
    }
}
