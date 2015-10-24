using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IIncomesRepository
        : IItemReaderProvider<Income>, IObservable<RepositoryChange<Income>>
    {
        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);
    }
}