using System;
using System.Collections.Generic;

namespace BillPath.Models
{
    public sealed class IncomeEqualityComparer
        : IEqualityComparer<Income>
    {
        private static volatile IncomeEqualityComparer _instance = null;

        public static IncomeEqualityComparer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IncomeEqualityComparer();

                return _instance;
            }
        }

        private IncomeEqualityComparer()
        {
        }

        public bool Equals(Income x, Income y)
        {
            if (x == null)
                return y == null;
            else
                return y != null && _AreEqual(x, y);
        }
        private bool _AreEqual(Income x, Income y)
            => x.Amount == y.Amount
            && x.DateRealized == y.DateRealized
            && string.Equals(x.Description, y.Description, StringComparison.Ordinal);

        public int GetHashCode(Income obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.Amount.GetHashCode()
                ^ obj.DateRealized.GetHashCode()
                ^ obj.Description?.GetHashCode() ?? 0;
        }
    }
}