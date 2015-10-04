using System;
using System.Collections.Concurrent;
using System.Reflection;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Xaml;
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
            var executingAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
            var typeAssembly = type.GetTypeInfo().Assembly;

            if (CoreWindow.GetForCurrentThread() != null)
                if (executingAssembly == typeAssembly)
                    return ResourceLoader.GetForCurrentView($"/{type.Name}");
                else
                    return ResourceLoader.GetForCurrentView($"{typeAssembly.GetName().Name}/{type.Name}");
            else if (executingAssembly == typeAssembly)
                return ResourceLoader.GetForViewIndependentUse($"/{type.Name}");
            else
                return ResourceLoader.GetForViewIndependentUse($"{typeAssembly.GetName().Name}/{type.Name}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}