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
        internal Currency(string isoCode, string symbol)
        {
            _isoCode = isoCode;
            _symbol = symbol;
        }

        public string IsoCode
            => _isoCode;
        public string Symbol
            => _symbol;

        public static bool operator ==(Currency left, Currency right)
            => left.Equals(right);
        public static bool operator !=(Currency left, Currency right)
            => !left.Equals(right);
        public bool Equals(Currency other)
            => _AreEqual(_isoCode, other._isoCode)
            && _AreEqual(_symbol, other._symbol);
        private bool _AreEqual(string x, string y)
        {
            if (string.IsNullOrWhiteSpace(x))
                return string.IsNullOrWhiteSpace(y);
            else
                return !string.IsNullOrWhiteSpace(y) && StringComparer.OrdinalIgnoreCase.Equals(x, y);
        }

        public override bool Equals(object obj)
        {
            var currency = obj as Currency?;
            return (currency != null && Equals(currency.Value));
        }
        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(_isoCode ?? string.Empty)
            ^ StringComparer.OrdinalIgnoreCase.GetHashCode(_symbol ?? string.Empty);

        public override string ToString()
            => $"{{{nameof(IsoCode)} = {_isoCode}, {nameof(Symbol)} = {_symbol}}}";
    }
}