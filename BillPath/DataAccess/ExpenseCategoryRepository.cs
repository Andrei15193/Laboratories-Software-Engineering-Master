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

        public Task Remove(ExpenseCategory category)
        {
            return Remove(category, CancellationToken.None);
        }
        public abstract Task Remove(ExpenseCategory category, CancellationToken cancellationToken);

        public Task Save(ExpenseCategory category)
        {
            return Save(category, CancellationToken.None);
        }
        public abstract Task Save(ExpenseCategory category, CancellationToken cancellationToken);

        public Task Update(ExpenseCategory oldCategory, ExpenseCategory newCategory)
        {
            return Update(oldCategory, newCategory, CancellationToken.None);
        }
        public abstract Task Update(ExpenseCategory oldCategory, ExpenseCategory newCategory, CancellationToken cancellationToken);
    }
}