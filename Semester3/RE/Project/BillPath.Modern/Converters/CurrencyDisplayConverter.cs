using System;
using System.Collections.Generic;
using BillPath.Models;
using Windows.UI.Xaml.Data;

namespace BillPath.Modern.Converters
{
    public class CurrencyDisplayConverter
        : IValueConverter
    {
        private static readonly IReadOnlyDictionary<CurrencyDisplayFormat, WeakReference<ICurrencyFormatter>> _currencyFormatterCache =
            new Dictionary<CurrencyDisplayFormat, WeakReference<ICurrencyFormatter>>
            {
                [CurrencyDisplayFormat.Full] = new WeakReference<ICurrencyFormatter>(null),
                [CurrencyDisplayFormat.IsoCode] = new WeakReference<ICurrencyFormatter>(null),
                [CurrencyDisplayFormat.Symbol] = new WeakReference<ICurrencyFormatter>(null)
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return _GetCurrencyFormatterFor((CurrencyDisplayFormat)value).Format((Currency)parameter);
        }
        private static ICurrencyFormatter _GetCurrencyFormatterFor(CurrencyDisplayFormat currencyDisplayFormat)
        {
            WeakReference<ICurrencyFormatter> cachedCurrencyFormatter;
            if (!_currencyFormatterCache.TryGetValue(currencyDisplayFormat, out cachedCurrencyFormatter))
                throw new ArgumentException(nameof(currencyDisplayFormat));

            ICurrencyFormatter currencyFormatter;
            if (!cachedCurrencyFormatter.TryGetTarget(out currencyFormatter))
            {
                currencyFormatter = _CreateCurrencyFormatterFor(currencyDisplayFormat);
                cachedCurrencyFormatter.SetTarget(currencyFormatter);
            }

            return currencyFormatter;
        }

        private static ICurrencyFormatter _CreateCurrencyFormatterFor(CurrencyDisplayFormat currencyDisplayFormat)
        {
            switch (currencyDisplayFormat)
            {
                case CurrencyDisplayFormat.Full:
                    return new CurrencySynmbolAndIsoCodeFormatter();

                case CurrencyDisplayFormat.Symbol:
                    return new CurrencySymbolOnlyFormatter();

                case CurrencyDisplayFormat.IsoCode:
                    return new CurrencyIsoCodeOnlyFormatter();

                default:
                    throw new ArgumentException(nameof(currencyDisplayFormat));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}