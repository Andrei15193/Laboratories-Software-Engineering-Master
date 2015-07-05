using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    public class BoolToVisibilityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibilityWhenTrue;
            if (!Enum.TryParse<Visibility>(System.Convert.ToString(parameter), true, out visibilityWhenTrue))
                throw new ArgumentException();

            if ((bool)value)
                return visibilityWhenTrue;
            else
                switch (visibilityWhenTrue)
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
            return (System.Convert.ToString(value).Equals(System.Convert.ToString(parameter), StringComparison.OrdinalIgnoreCase));
        }
    }
}