using System;
using System.Collections.Concurrent;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class LocalizationConverter
        : IValueConverter
    {
        private readonly ConcurrentDictionary<Type, ResourceLoader> _resourceLoaderCache =
            new ConcurrentDictionary<Type, ResourceLoader>();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var resourceLoader = _resourceLoaderCache.GetOrAdd(value.GetType(), _GetResourceLoaderFor);
            if (resourceLoader == null)
                return System.Convert.ToString(value);

            return resourceLoader.GetString(System.Convert.ToString(parameter ?? value));
        }
        private static ResourceLoader _GetResourceLoaderFor(Type type)
        {
            return ResourceLoader.GetForViewIndependentUse($"/{type.Name}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}