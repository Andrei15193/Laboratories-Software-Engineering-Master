using System;
using BillPath.Models;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class ArgbColorToColorConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var argbColor = (ArgbColor)value;

            return Color.FromArgb(argbColor.Alpha, argbColor.Red, argbColor.Green, argbColor.Blue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var color = (Color)value;

            return new ArgbColor(color.A, color.R, color.G, color.B);
        }
    }
}