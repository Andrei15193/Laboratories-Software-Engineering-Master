using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class ToEnumerableConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => Enumerable.Repeat(value, parameter as int? ?? 1);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}