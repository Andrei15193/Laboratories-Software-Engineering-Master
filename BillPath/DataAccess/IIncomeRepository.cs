using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IIncomeRepository
    {
        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);
    }
}