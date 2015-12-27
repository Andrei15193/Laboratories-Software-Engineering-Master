using System;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class ModelFromModelStateConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((ModelState)value).Model;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => value;
    }
}