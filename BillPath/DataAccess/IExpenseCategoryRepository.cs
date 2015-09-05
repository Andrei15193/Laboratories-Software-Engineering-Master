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

        Task Save(ExpenseCategory category);
        Task Save(ExpenseCategory category, CancellationToken cancellationToken);

        Task Update(ExpenseCategory oldCategory, ExpenseCategory newCategory);
        Task Update(ExpenseCategory oldCategory, ExpenseCategory newCategory, CancellationToken cancellationToken);

        Task Remove(ExpenseCategory category);
        Task Remove(ExpenseCategory category, CancellationToken cancellationToken);
    }
}