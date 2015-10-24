using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public abstract class IncomeRepository
        : IIncomeRepository
    {
        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public abstract Task SaveAsync(Income income, CancellationToken cancellationToken);

        public abstract IItemReader<Income> GetReader();

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public abstract Task<int> GetItemCountAsync(CancellationToken cancellationToken);

        public IDisposable Subscribe(IObserver<IncomeRepositoryChanged> observer)
        {
            throw new NotImplementedException();
        }
    }
}