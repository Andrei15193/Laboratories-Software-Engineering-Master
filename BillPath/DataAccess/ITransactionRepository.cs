using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface ITransactionRepository<TTransaction>
        where TTransaction : Transaction
    {
        Task<IEnumerable<TTransaction>> GetForAsync(Account account, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<TTransaction>> GetForAsync(Account account, int pageNumber);
        Task<int> GetPageCountAsync(Account account);

        Task SaveAsync(TTransaction transaction);

        Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction);

        Task RemoveAsync(TTransaction transaction);
    }
}