using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public interface IIncomeXmlReader
        : IDisposable
    {
        Income Current
        {
            get;
        }

        Task<bool> ReadAsync();
        Task<bool> ReadAsync(CancellationToken cancellationToken);

        Task SkipAsync(int count);
        Task SkipAsync(int count, CancellationToken cancellationToken);
    }
}