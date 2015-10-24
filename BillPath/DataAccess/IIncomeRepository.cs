using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IIncomeRepository
        : IItemReaderProvider<Income>, IObservable<IncomeRepositoryChange>
    {
        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);
    }
}