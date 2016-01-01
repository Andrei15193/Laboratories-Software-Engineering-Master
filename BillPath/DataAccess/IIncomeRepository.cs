using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IIncomeRepository
    {
        Task<IIncomeReader> GetReaderAsync();
        Task<IIncomeReader> GetReaderAsync(CancellationToken cancellationToken);

        Task<int> GetCountAsync();
        Task<int> GetCountAsync(CancellationToken cancellationToken);

        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);

        Task RemoveAsync(Income income);
        Task RemoveAsync(Income income, CancellationToken cancellationToken);
    }
}