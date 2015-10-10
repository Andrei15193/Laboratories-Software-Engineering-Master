using System.Collections.Generic;
using BillPath.Models;

namespace BillPath.Providers
{
    public class CurrencyDisplayFormatProvider
    {
        public IEnumerable<CurrencyDisplayFormat> CurrencyDisplayFormats
        {
            get
            {
                yield return CurrencyDisplayFormat.Full;
                yield return CurrencyDisplayFormat.Symbol;
                yield return CurrencyDisplayFormat.IsoCode;
            }
        }
    }
}