using System;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public class IncomeRepositoryChanged
    {
        public IncomeRepositoryChanged(Income income)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            Income = income;
        }

        public Income Income { get; }
    }
    public enum IncomeRepositoryChange
    {
        Add
    }
}