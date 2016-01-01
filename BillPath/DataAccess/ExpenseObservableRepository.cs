using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public class ExpenseObservableRepository
        : IExpenseRepository
    {
        private readonly IExpenseRepository _repository;

        public ExpenseObservableRepository(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public event EventHandler SavedIncome;
        public event EventHandler RemovedIncome;

        public Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate)
            => GetReaderAsync(predicate, CancellationToken.None);
        public Task<IExpenseReader> GetReaderAsync(Predicate<Expense> predicate, CancellationToken cancellationToken)
            => _repository.GetReaderAsync(predicate, cancellationToken);

        public Task<int> GetCountAsync(Predicate<Expense> predicate)
            => GetCountAsync(predicate, CancellationToken.None);
        public Task<int> GetCountAsync(Predicate<Expense> predicate, CancellationToken cancellationToken)
            => _repository.GetCountAsync(predicate, cancellationToken);

        public Task SaveAsync(Expense expense)
            => SaveAsync(expense, CancellationToken.None);
        public async Task SaveAsync(Expense expense, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync(expense, cancellationToken);
            SavedIncome?.Invoke(this, EventArgs.Empty);
        }
        public Task RemoveAsync(Expense expense)
            => RemoveAsync(expense, CancellationToken.None);
        public async Task RemoveAsync(Expense expense, CancellationToken cancellationToken)
        {
            await _repository.RemoveAsync(expense, cancellationToken);
            RemovedIncome?.Invoke(this, EventArgs.Empty);
        }

        public Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory)
            => UpdateCategory(predicate, expenseCategory, CancellationToken.None);
        public async Task UpdateCategory(Predicate<Expense> predicate, ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            await _repository.UpdateCategory(predicate, expenseCategory, cancellationToken);
            SavedIncome?.Invoke(this, EventArgs.Empty);
        }
    }
}