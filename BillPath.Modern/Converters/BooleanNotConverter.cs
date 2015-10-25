using System;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class BooleanNotConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }
}