using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class ResourceFormatConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var resourceLocation = System.Convert.ToString(parameter).Split(new[] { ' ' }, 2);

            if (resourceLocation.Length != 2)
                throw new ArgumentException(
                    "The parameter must specify resource location in the form '<filePath> <stringKey>'",
                    nameof(parameter));

            var resourceLoader = ResourceLoader.GetForViewIndependentUse(resourceLocation[0]);
            return string.Format(resourceLoader.GetString(resourceLocation[1]), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}