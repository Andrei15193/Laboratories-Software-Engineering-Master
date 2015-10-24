using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class NullableIntToStringConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var nullableInt = value as int?;

            return nullableInt == null
                ? string.Empty
                : System.Convert.ToString(nullableInt.Value, _GetFormatProviderFor(language));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int intValue;
            if (int.TryParse(
                System.Convert.ToString(value),
                NumberStyles.Any,
                _GetFormatProviderFor(language),
                out intValue))
                return intValue;
            else
                return null;
        }

        private IFormatProvider _GetFormatProviderFor(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return CultureInfo.CurrentCulture;
            else
                return new CultureInfo(language);
        }
    }
}