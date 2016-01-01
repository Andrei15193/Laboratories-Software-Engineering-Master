using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public class ExpenseCategoryObservableRepository
        : IExpenseCategoryRepository
    {
        private readonly IExpenseCategoryRepository _repository;

        public ExpenseCategoryObservableRepository(IExpenseCategoryRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public event EventHandler<ExpenseCategory> SavedExpenseCategory;
        public event EventHandler<ExpenseCategory> RemovedExpenseCategory;

        public Task<IEnumerable<ExpenseCategory>> GetAllAsync()
            => GetAllAsync(CancellationToken.None);
        public Task<IEnumerable<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken)
            => _repository.GetAllAsync(cancellationToken);

        public Task SaveAsync(ExpenseCategory expenseCategory)
            => SaveAsync(expenseCategory, CancellationToken.None);
        public async Task SaveAsync(ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            await _repository.SaveAsync(expenseCategory, cancellationToken);
            SavedExpenseCategory?.Invoke(this, expenseCategory);
        }
    }
}