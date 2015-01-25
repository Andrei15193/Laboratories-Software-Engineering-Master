using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class BoolVisibilityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((Visibility)value == Visibility.Visible);
        }
    }
}