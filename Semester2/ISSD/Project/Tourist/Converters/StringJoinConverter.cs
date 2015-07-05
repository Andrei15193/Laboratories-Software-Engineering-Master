using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class StringJoinConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Join((string)parameter, (IEnumerable<string>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}