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
    }
}