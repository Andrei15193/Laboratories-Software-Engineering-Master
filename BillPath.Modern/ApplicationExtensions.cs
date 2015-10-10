using System;
using Windows.UI.Xaml;

namespace BillPath.Modern
{
    public static class ApplicationExtensions
    {
        public static object GetResource(this Application application, string key)
        {
            return GetResource<object>(application, key);
        }
        public static TResource GetResource<TResource>(this Application application, string key)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrWhiteSpace(key))
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(key));

#if DEBUG
            object resource;
            if (application.Resources.TryGetValue("DEBUG_" + key, out resource))
                return (TResource)resource;
#endif
            return (TResource)application.Resources[key];
        }
    }
}