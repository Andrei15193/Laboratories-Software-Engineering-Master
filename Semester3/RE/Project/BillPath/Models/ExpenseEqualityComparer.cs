using System;
using System.Collections.Generic;

namespace BillPath.Models
{
    public sealed class ExpenseEqualityComparer
        : IEqualityComparer<Expense>
    {
        private static volatile ExpenseEqualityComparer _instance = null;

        public static ExpenseEqualityComparer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ExpenseEqualityComparer();

                return _instance;
            }
        }

        private ExpenseEqualityComparer()
        {
        }

        public bool Equals(Expense x, Expense y)
        {
            if (x == null)
                return y == null;
            else
                return y != null && _AreEqual(x, y);
        }
        private bool _AreEqual(Expense x, Expense y)
            => x.Amount == y.Amount
            && x.DateRealized == y.DateRealized
            && string.Equals(x.Description, y.Description, StringComparison.Ordinal)
            && string.Equals(x.Category?.Name, y.Category?.Name, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(Expense obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return obj.Amount.GetHashCode()
                ^ obj.DateRealized.GetHashCode()
                ^ obj.Description?.GetHashCode() ?? 0;
        }
    }
}