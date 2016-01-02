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

        Task SaveAsync(ExpenseCategory expenseCategory);
        Task SaveAsync(ExpenseCategory expenseCategory, CancellationToken cancellationToken);

        Task RemoveAsync(string name);
        Task RemoveAsync(string name, CancellationToken cancellationToken);

        Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory);
        Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory, CancellationToken cancellationToken);
    }
}