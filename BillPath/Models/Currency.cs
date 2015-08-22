using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public struct Currency
        : IEquatable<Currency>
    {
        [DataMember(Name = nameof(IsoCode), IsRequired = true)]
        private readonly string _isoCode;
        [DataMember(Name = nameof(Symbol), IsRequired = true)]
        private readonly string _symbol;

        public Currency(RegionInfo regionInfo)
        {
            if (regionInfo == null)
                throw new ArgumentNullException(nameof(regionInfo));

            _isoCode = regionInfo.ISOCurrencySymbol;
            _symbol = regionInfo.CurrencySymbol;
        }

        public string IsoCode
        {
            get
            {
                return _isoCode;
            }
        }
        public string Symbol
        {
            get
            {
                return _symbol;
            }
        }

        public static bool operator ==(Currency left, Currency right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Currency left, Currency right)
        {
            return !left.Equals(right);
        }
        public bool Equals(Currency other)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(_isoCode, other._isoCode)
                   && StringComparer.OrdinalIgnoreCase.Equals(_symbol, other._symbol);
        }
        public override bool Equals(object obj)
        {
            var currency = obj as Currency?;
            return (currency != null && Equals(currency.Value));
        }
        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_isoCode ?? string.Empty)
                   ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_symbol ?? string.Empty);
        }

        public override string ToString()
        {
            return $"{{{nameof(IsoCode)} = {_isoCode}, {nameof(Symbol)} = {_symbol}}}";
        }
    }
}