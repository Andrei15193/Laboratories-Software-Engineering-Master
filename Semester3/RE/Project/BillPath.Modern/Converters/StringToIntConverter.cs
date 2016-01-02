using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class StringToIntConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (language == null)
                return System.Convert.ToInt32(value);
            else
                return System.Convert.ToInt32(value, new CultureInfo(language));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (language == null)
                return System.Convert.ToString(value);
            else
                return System.Convert.ToString(value, new CultureInfo(language));
        }
    }
}