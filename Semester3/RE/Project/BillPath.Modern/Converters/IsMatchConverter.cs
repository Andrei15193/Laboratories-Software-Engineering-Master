using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class IsMatchConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Regex.IsMatch((string)value, (string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}