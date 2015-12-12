using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public interface IIncomeXmlRepository
    {
        Task<IIncomeXmlReader> GetReaderAsync();
        Task<IIncomeXmlReader> GetReaderAsync(CancellationToken cancellationToken);

        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);

        Task RemoveAsync(Income income);
        Task RemoveAsync(Income income, CancellationToken cancellationToken);
    }
}