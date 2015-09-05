using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class ExpenseRepository
        : TransactionRepository<Expense>, IExpenseRepository
    {
        public Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, int pageNumber)
        {
            return GetInAsync(currency, category, pageNumber, CancellationToken.None);
        }
        public abstract Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, int pageNumber, CancellationToken cancellationToken);

        public Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end)
        {
            return GetInAsync(currency, category, start, end, CancellationToken.None);
        }
        public abstract Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);

        public Task<int> GetPageCountAsync(Currency currency, ExpenseCategory category)
        {
            return GetPageCountAsync(currency, category, CancellationToken.None);
        }
        public abstract Task<int> GetPageCountAsync(Currency currency, ExpenseCategory category, CancellationToken cancellationToken);
    }
}