using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class TransactionRepository<TTransaction>
        : ITransactionRepository<TTransaction>
        where TTransaction : Transaction<TTransaction>
    {
        public Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, int pageNumber)
        {
            return GetInAsync(currency, pageNumber, CancellationToken.None);
        }
        public abstract Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, int pageNumber, CancellationToken cancellationToken);

        public Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, DateTimeOffset start, DateTimeOffset end)
        {
            return GetInAsync(currency, start, end, CancellationToken.None);
        }
        public abstract Task<IEnumerable<TTransaction>> GetInAsync(Currency currency, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken);

        public Task<int> GetPageCountAsync(Currency currency)
        {
            return GetPageCountAsync(currency, CancellationToken.None);
        }
        public abstract Task<int> GetPageCountAsync(Currency currency, CancellationToken cancellationToken);
        public Task<int> GetCountAsync(Currency currency)
        {
            return GetCountAsync(currency, CancellationToken.None);
        }
        public virtual async Task<int> GetCountAsync(Currency currency, CancellationToken cancellationToken)
        {
            return
                (await Task.WhenAll(
                    from pageNumber in Enumerable.Range(1, await GetPageCountAsync(currency, cancellationToken))
                    select GetInAsync(currency, pageNumber, cancellationToken)
                        .ContinueWith(transactions => transactions.Result.Count(), cancellationToken)))
                .DefaultIfEmpty()
                .Sum();

        }

        public Task RemoveAsync(TTransaction transaction)
        {
            return RemoveAsync(transaction, CancellationToken.None);
        }
        public abstract Task RemoveAsync(TTransaction transaction, CancellationToken cancellationToken);

        public Task SaveAsync(TTransaction transaction)
        {
            return SaveAsync(transaction, CancellationToken.None);
        }
        public abstract Task SaveAsync(TTransaction transaction, CancellationToken cancellationToken);

        public Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction)
        {
            return UpdateAsync(oldTransaction, newTransaction, CancellationToken.None);
        }
        public abstract Task UpdateAsync(TTransaction oldTransaction, TTransaction newTransaction, CancellationToken cancellationToken);
    }
}