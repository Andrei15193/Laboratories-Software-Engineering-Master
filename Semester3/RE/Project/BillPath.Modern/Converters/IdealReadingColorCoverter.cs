using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class IdealReadingColorCoverter
        : IValueConverter
    {
        private const int _threshold = 106;
        private static readonly IValueConverter _argbColorToColorConverter = new ArgbColorToColorConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = (Color)_argbColorToColorConverter.Convert(value, typeof(Color), parameter, language);
            var colorDelta = System.Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));

            return (255 - colorDelta < _threshold) ? Colors.Black : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}