using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseCategoryRepository
    {
        Task<IEnumerable<ExpenseCategory>> GetAllAsync();
        Task<IEnumerable<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken);

        Task SaveAsync(ExpenseCategory category);
        Task SaveAsync(ExpenseCategory category, CancellationToken cancellationToken);

        Task UpdateAsync(ExpenseCategory oldCategory, ExpenseCategory newCategory);
        Task UpdateAsync(ExpenseCategory oldCategory, ExpenseCategory newCategory, CancellationToken cancellationToken);

        Task RemoveAsync(ExpenseCategory category);
        Task RemoveAsync(ExpenseCategory category, CancellationToken cancellationToken);
    }
}