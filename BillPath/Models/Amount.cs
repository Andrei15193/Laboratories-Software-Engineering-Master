using System;

namespace BillPath.Models
{
    public struct Amount
        : IEquatable<Amount>, IComparable<Amount>
    {
        public Amount(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        public decimal Value
        {
            get;
        }
        public Currency Currency
        {
            get;
        }

        public bool Equals(Amount other)
            => (Value == other.Value && Currency == other.Currency);
        public override bool Equals(object obj)
        {
            var other = obj as Amount?;

            return (other != null && Equals(other.Value));
        }
        public override int GetHashCode()
            => Value.GetHashCode() ^ Currency.GetHashCode();
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