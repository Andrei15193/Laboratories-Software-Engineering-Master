using System.Collections.Generic;
using BillPath.Models;

namespace BillPath.Providers
{
    public class CurrencyDisplayFormatProvider
    {
        public IEnumerable<CurrencyDisplayFormat> CurrencyDisplayFormats { get; } = new[]
            {
                CurrencyDisplayFormat.Full,
                CurrencyDisplayFormat.Symbol,
                CurrencyDisplayFormat.IsoCode
            };
    }
}