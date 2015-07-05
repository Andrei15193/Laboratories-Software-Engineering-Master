using System;
using System.Globalization;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class StringFormatConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string format = System.Convert.ToString(parameter);
            Type sourceType = value.GetType();
            CultureInfo cultureInfo = (string.IsNullOrWhiteSpace(language) ? CultureInfo.CurrentUICulture : new CultureInfo(language));

            if (sourceType == typeof(byte))
                return ((byte)value).ToString(format, cultureInfo);
            if (sourceType == typeof(sbyte))
                return ((sbyte)value).ToString(format, cultureInfo);

            if (sourceType == typeof(short))
                return ((short)value).ToString(format, cultureInfo);
            if (sourceType == typeof(ushort))
                return ((ushort)value).ToString(format, cultureInfo);

            if (sourceType == typeof(int))
                return ((int)value).ToString(format, cultureInfo);
            if (sourceType == typeof(uint))
                return ((uint)value).ToString(format, cultureInfo);

            if (sourceType == typeof(long))
                return ((long)value).ToString(format, cultureInfo);
            if (sourceType == typeof(ulong))
                return ((ulong)value).ToString(format, cultureInfo);

            if (sourceType == typeof(float))
                return ((float)value).ToString(format, cultureInfo);
            if (sourceType == typeof(double))
                return ((double)value).ToString(format, cultureInfo);
            if (sourceType == typeof(decimal))
                return ((decimal)value).ToString(format, cultureInfo);

            if (sourceType == typeof(DateTime))
                return ((DateTime)value).ToString(format, cultureInfo);
            if (sourceType == typeof(DateTimeOffset))
                return ((DateTimeOffset)value).ToString(format, cultureInfo);

            return System.Convert.ToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}