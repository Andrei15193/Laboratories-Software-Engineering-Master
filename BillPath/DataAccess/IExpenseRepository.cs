using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseRepository
        : ITransactionRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetForAsync(Currency currency, ExpenseCategory category, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<Expense>> GetForAsync(Currency currency, ExpenseCategory category, int pageNumber);
        Task<int> GetPageCountAsync(Currency currency, ExpenseCategory category);
    }
}