using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseRepository
        : ITransactionRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetForAsync(Account account, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<Expense>> GetForAsync(Account account, ExpenseCategory category, int pageNumber);
        Task<int> GetPageCountAsync(Account account, ExpenseCategory category);
    }
}