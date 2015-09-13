using System;
using BillPath.Models;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class CurrencyDisplayConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return CurrencyDisplayFormatters
                .GetFor(value as CurrencyDisplayFormat? ?? default(CurrencyDisplayFormat))
                .Format((Currency)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}