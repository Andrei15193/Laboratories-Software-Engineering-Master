using System;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public class IncomeRepositoryChange
    {
        public IncomeRepositoryChange(Income income, IncomeRepositoryChangeAction action)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            Income = income;
            Action = action;
        }

        public Income Income
        {
            get;
        }
        public IncomeRepositoryChangeAction Action
        {
            get;
        }
    }
}