using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class NullToVisibilityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibilityWhenNull;
            if (!Enum.TryParse<Visibility>(System.Convert.ToString(parameter), true, out visibilityWhenNull))
                throw new ArgumentException();

            if (value == null)
                return visibilityWhenNull;
            else
                switch (visibilityWhenNull)
                {
                    case Visibility.Collapsed:
                        return Visibility.Visible;

                    case Visibility.Visible:
                        return Visibility.Collapsed;

                    default:
                        throw new ArgumentException();
                }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}