using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BillPath.UserInterface.Converters
{
    public class ReadableColorConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color = (Color)value;

            if ((1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255) < 0.5)
                return Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            else
                return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}