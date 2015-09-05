using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseRepository
        : ITransactionRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, int pageNumber);
        Task<IEnumerable<Expense>> GetInAsync(Currency currency, ExpenseCategory category, int pageNumber, CancellationToken cancellationToken);
        Task<int> GetPageCountAsync(Currency currency, ExpenseCategory category);
        Task<int> GetPageCountAsync(Currency currency, ExpenseCategory category, CancellationToken cancellationToken);
    }
}