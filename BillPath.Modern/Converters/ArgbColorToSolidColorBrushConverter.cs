using System;
using BillPath.Models;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BillPath.Modern.Converters
{
    public class ArgbColorToSolidColorBrushConverter
        : IValueConverter
    {
        private static readonly ArgbColorToColorConverter _argbToColorConverter = new ArgbColorToColorConverter();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush((Color)_argbToColorConverter.Convert(
                value,
                targetType,
                parameter,
                language));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (ArgbColor)_argbToColorConverter.ConvertBack(
                ((SolidColorBrush)value).Color,
                targetType,
                parameter,
                language);
        }
    }
}