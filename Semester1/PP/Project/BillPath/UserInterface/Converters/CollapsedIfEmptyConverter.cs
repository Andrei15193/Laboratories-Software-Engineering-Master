using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BillPath.UserInterface.Converters
{
    public class CollapsedIfEmptyConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IEnumerator enumerator = ((IEnumerable)value).GetEnumerator();
            try
            {
            if (enumerator.MoveNext())
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
            }
            finally
            {
                IDisposable disposableEnumerator = enumerator as IDisposable;
                if (disposableEnumerator != null)
                    disposableEnumerator.Dispose();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}