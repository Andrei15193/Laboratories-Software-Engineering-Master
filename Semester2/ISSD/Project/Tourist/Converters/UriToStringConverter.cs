
using System;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class UriToStringConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var uri = value as Uri;

            if (uri != null)
                return uri.ToString();
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var @string = value as string;

            if (@string != null)
                return new Uri(@string, UriKind.Absolute);
            else
                return null;
        }
    }
}