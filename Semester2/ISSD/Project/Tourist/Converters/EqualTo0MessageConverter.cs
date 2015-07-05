using System;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class EqualTo0MessageConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (System.Convert.ToInt32(value) == 0)
                return parameter;
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}