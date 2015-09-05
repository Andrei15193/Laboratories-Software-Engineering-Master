using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class ExpenseCategoryRepository
        : IExpenseCategoryRepository
    {
        public Task<IEnumerable<ExpenseCategory>> GetAllAsync()
        {
            return GetAllAsync(CancellationToken.None);
        }
        public abstract Task<IEnumerable<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken);

        public Task RemoveAsync(ExpenseCategory category)
        {
            return RemoveAsync(category, CancellationToken.None);
        }
        public abstract Task RemoveAsync(ExpenseCategory category, CancellationToken cancellationToken);

        public Task SaveAsync(ExpenseCategory category)
        {
            return SaveAsync(category, CancellationToken.None);
        }
        public abstract Task SaveAsync(ExpenseCategory category, CancellationToken cancellationToken);

        public Task UpdateAsync(ExpenseCategory oldCategory, ExpenseCategory newCategory)
        {
            return UpdateAsync(oldCategory, newCategory, CancellationToken.None);
        }
        public abstract Task UpdateAsync(ExpenseCategory oldCategory, ExpenseCategory newCategory, CancellationToken cancellationToken);
    }
}