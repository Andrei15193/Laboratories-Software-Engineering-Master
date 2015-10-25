using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class DecimalToStringConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => ((decimal)value).ToString(
                parameter as string,
                _GetFormatProviderFor(language));

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return decimal.Parse(
                (string)value,
                NumberStyles.Any,
                _GetFormatProviderFor(language));
        }

        private IFormatProvider _GetFormatProviderFor(string language)
            => string.IsNullOrWhiteSpace(language) ? CultureInfo.CurrentCulture : new CultureInfo(language);
    }
}