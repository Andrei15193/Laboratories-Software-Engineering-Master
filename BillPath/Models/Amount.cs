using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public struct Amount
        : IEquatable<Amount>, IComparable<Amount>
    {
        [DataMember(Name = nameof(Value))]
        private readonly decimal _value;
        [DataMember(Name = nameof(Currency))]
        private readonly Currency _currency;

        public Amount(decimal value, Currency currency)
        {
            _value = value;
            _currency = currency;
        }

        public decimal Value
            => _value;
        public Currency Currency
            => _currency;

        public override string ToString()
            => $"{{{nameof(Value)} = {Value}, {nameof(Currency)} = {Currency}}}";

        public bool Equals(Amount other)
            => Value == other.Value
            && Currency == other.Currency;
        public override bool Equals(object obj)
        {
            var other = obj as Amount?;

            return (other != null && Equals(other.Value));
        }
        public override int GetHashCode()
            => Value.GetHashCode()
            ^ Currency.GetHashCode();
        public static bool operator ==(Amount left, Amount right)
            => left.Equals(right);
        public static bool operator !=(Amount left, Amount right)
            => !left.Equals(right);

        public int CompareTo(Amount other)
        {
            _ValidateCurrency(this, other, nameof(other));

            return Value.CompareTo(other.Value);
        }
        public static bool operator <(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return left.CompareTo(right) < 0;
        }
        public static bool operator >(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return left.CompareTo(right) > 0;
        }
        public static bool operator <=(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return (left == right || left < right);
        }
        public static bool operator >=(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return (left == right || left > right);
        }

        public static Amount operator +(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return new Amount(left.Value + right.Value, left.Currency);
        }
        public static Amount operator -(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return new Amount(left.Value - right.Value, left.Currency);
        }
        public static Amount operator *(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return new Amount(left.Value * right.Value, left.Currency);
        }
        public static Amount operator /(Amount left, Amount right)
        {
            _ValidateCurrency(left, right);

            return new Amount(left.Value / right.Value, left.Currency);
        }

        private static void _ValidateCurrency(Amount first, Amount second, string paramName = null)
        {
            if (first.Currency != second.Currency)
                throw new ArgumentException("Can only add amounts expressed in same currency", paramName);
        }
    }
}