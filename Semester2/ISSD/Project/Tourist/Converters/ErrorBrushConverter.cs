using System;
using Windows.UI.Xaml.Data;
namespace Tourist.Converters
{
    internal class Int32EqualityConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (System.Convert.ToInt32(value) == System.Convert.ToInt32(parameter ?? NullParameterValue))
                return AreEqualValue;
            else
                return AreNotEqualValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object NullParameterValue
        {
            get;
            set;
        }
        public object AreNotEqualValue
        {
            get;
            set;
        }
        public object AreEqualValue
        {
            get;
            set;
        }
    }
}