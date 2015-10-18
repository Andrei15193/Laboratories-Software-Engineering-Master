using System;
using System.Threading;
using System.Threading.Tasks;

namespace BillPath.DataAccess
{
    public interface IItemReader<TItem>
        : IDisposable
    {
        Task<bool> ReadAsync();
        Task<bool> ReadAsync(CancellationToken cancellationToken);

        TItem Current
        {
            get;
        }
    }
}