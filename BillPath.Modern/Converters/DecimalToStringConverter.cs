using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class DecimalToStringConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var decimalValue = (decimal)value;
            var formatOptions = (parameter as string)?.Split(new[] { '/' }, 2);

            if (formatOptions == null)
                return decimalValue.ToString(null, _GetFormatProviderFor(language));
            else if (formatOptions.Length == 1
                || decimalValue == 0
                || decimalValue >= 0.01M)
                return decimalValue.ToString(formatOptions[0], _GetFormatProviderFor(language));
            else
                return decimalValue.ToString(formatOptions[1], _GetFormatProviderFor(language));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => decimal.Parse(
                (string)value,
                NumberStyles.Any,
                _GetFormatProviderFor(language));

        private IFormatProvider _GetFormatProviderFor(string language)
            => string.IsNullOrWhiteSpace(language) ? CultureInfo.CurrentCulture : new CultureInfo(language);
    }
}