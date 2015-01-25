using BillPath.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    internal class ColorNameConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return NamedColors.GetByName((string)value).Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return NamedColors.GetByColor((Color)value).Name;
        }
    }
}