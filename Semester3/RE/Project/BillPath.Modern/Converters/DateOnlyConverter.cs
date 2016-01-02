using System;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class DateOnlyConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((DateTimeOffset)value).ToString("d");

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}