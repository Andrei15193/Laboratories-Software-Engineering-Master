using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IIncomeRepository
    {
        Task SaveAsync(Income income);
        Task SaveAsync(Income income, CancellationToken cancellationToken);

        [Obsolete]
        Task<IEnumerable<Income>> GetAllAsync();
        [Obsolete]
        Task<IEnumerable<Income>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<Income>> GetOnPageAsync(int pageNumber);
        Task<IEnumerable<Income>> GetOnPageAsync(int pageNumber, CancellationToken cancellationToken);

        Task<int> GetPageCountAsync();
        Task<int> GetPageCountAsync(CancellationToken cancellationToken);
    }
}