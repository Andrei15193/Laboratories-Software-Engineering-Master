using BillPath.Models;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class ValidationErrorConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ValidationError validationError = (ValidationError)value;

            return ResourceLoader.GetForViewIndependentUse(validationError.DeclaringType.Name).GetString(string.Format("{0}%20error%20{1}", validationError.PropertyName, validationError.ErrorId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}