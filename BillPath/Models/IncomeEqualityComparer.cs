using System;
using System.Collections.Generic;

namespace BillPath.Models
{
    public class IncomeEqualityComparer
        : IEqualityComparer<Income>
    {
        public static IncomeEqualityComparer Instance { get; } = new IncomeEqualityComparer();

        public bool Equals(Income x, Income y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null)
                return (y == null);
            else
                return (y != null && _Equals(x, y));
        }

        public int GetHashCode(Income obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return (obj.Amount.GetHashCode()
                    ^ obj.DateRealized.GetHashCode()
                    ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Description ?? string.Empty));
        }

        private bool _Equals(Income x, Income y)
        {
            return (x.Amount == y.Amount
                    && x.DateRealized == y.DateRealized
                    && StringComparer.OrdinalIgnoreCase.Equals(x.Description, y.Description));
        }
    }
}