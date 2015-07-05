using System;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class NotEqualToInt32ParameterConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !System.Convert.ToInt32(value).Equals(System.Convert.ToInt32(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}