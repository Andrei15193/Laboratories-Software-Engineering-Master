using System;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class ModelStateToModelOnlyConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => ((ModelState)value)?.Model;
    }
}