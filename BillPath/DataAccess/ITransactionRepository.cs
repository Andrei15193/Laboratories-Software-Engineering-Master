using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface ITransactionRepository<TTransaction>
        where TTransaction : Transaction
    {
        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, int pageNumber);
        Task<int> GetPageCountAsync(Currency currency);

        Task SaveAsync(TTransaction transaction);

        Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction);

        Task RemoveAsync(TTransaction transaction);
    }
}