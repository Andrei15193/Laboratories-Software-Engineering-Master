using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface ITransactionRepository<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, DateTimeOffset start, DateTimeOffset end);
        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);

        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, int pageNumber);
        Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, int pageNumber, CancellationToken cancellationToken);

        Task<int> GetPageCountAsync(Currency currency);
        Task<int> GetPageCountAsync(Currency currency, CancellationToken cancellationToken);

        Task SaveAsync(TTransaction transaction);
        Task SaveAsync(TTransaction transaction, CancellationToken cancellationToken);

        Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction);
        Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction, CancellationToken cancellationToken);

        Task RemoveAsync(TTransaction transaction);
        Task RemoveAsync(TTransaction transaction, CancellationToken cancellationToken);
    }
}