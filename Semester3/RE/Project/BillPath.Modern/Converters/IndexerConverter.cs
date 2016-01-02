using System;
using System.Collections;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class IndexerConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((IDictionary)value)[parameter];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}