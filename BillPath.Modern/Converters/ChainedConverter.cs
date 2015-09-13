using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class ChainedConverter
        : IValueConverter
    {
        public IList<IValueConverter> Converters { get; } = new List<IValueConverter>();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            foreach (var converter in Converters)
                value = converter.Convert(value, targetType, parameter, language);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            foreach (var converter in Converters)
                value = converter.ConvertBack(value, targetType, parameter, language);

            return value;
        }
    }
}