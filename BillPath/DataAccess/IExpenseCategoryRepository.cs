using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseCategoryRepository
    {
        Task<IEnumerable<ExpenseCategory>> GetAllAsync();

        Task Save(ExpenseCategory category);

        Task Update(ExpenseCategory oldCategory, ExpenseCategory newCategory);

        Task Remove(ExpenseCategory category);
    }
}