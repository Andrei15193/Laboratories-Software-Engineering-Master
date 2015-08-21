using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class IdealReadingColorCoverter
        : IValueConverter
    {
        private const int _threshold = 105;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = (Color)value;
            var colorDelta = System.Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));

            return (255 - colorDelta < _threshold) ? Colors.Black : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}