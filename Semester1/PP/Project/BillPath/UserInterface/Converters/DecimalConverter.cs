using System;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class DecimalConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value ?? parameter).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            decimal decimalValue;

            if (decimal.TryParse((string)value, out decimalValue))
                return decimalValue;
            else if (parameter is decimal)
                return parameter;
            else
                return decimal.Parse((string)parameter);
        }
    }
}