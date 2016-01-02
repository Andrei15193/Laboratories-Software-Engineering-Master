using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess
{
    public interface IExpenseReader
        : IDisposable
    {
        Expense Current
        {
            get;
        }

        Task<bool> ReadAsync();
        Task<bool> ReadAsync(CancellationToken cancellationToken);

        Task SkipAsync(int count);
        Task SkipAsync(int count, CancellationToken cancellationToken);
    }
}