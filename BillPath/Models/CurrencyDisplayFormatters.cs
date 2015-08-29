using System;

namespace BillPath.Models
{
    public static class CurrencyDisplayFormatters
    {
        public static ICurrencyFormatter GetFor(CurrencyDisplayFormat currencyDisplayFormat)
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
                    throw new NotImplementedException();
            }
        }
    }
}