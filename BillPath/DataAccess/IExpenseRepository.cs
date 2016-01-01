using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseRepository
    {
        Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate);
        Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate, CancellationToken cancellationToken);

        Task<int> GetCountAsync(Predicate<Expense> predicate);
        Task<int> GetCountAsync(Predicate<Expense> predicate, CancellationToken cancellationToken);

        Task SaveAsync(Expense expense);
        Task SaveAsync(Expense expense, CancellationToken cancellationToken);

        Task RemoveAsync(Expense expense);
        Task RemoveAsync(Expense expense, CancellationToken cancellationToken);

        Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory);
        Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory, CancellationToken cancellationToken);
    }
}