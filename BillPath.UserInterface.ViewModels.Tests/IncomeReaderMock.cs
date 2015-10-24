using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels.Tests
{
    internal class IncomeReaderMock
        : IItemReader<Income>
    {
        private readonly IEnumerator<Income> _incomeEnumerator;

        public IncomeReaderMock(IEnumerator<Income> incomeEnumerator)
        {
            _incomeEnumerator = incomeEnumerator;
        }

        public Income Current
            => _incomeEnumerator.Current;

        public void Dispose()
            => _incomeEnumerator.Dispose();

        public Task<bool> ReadAsync()
            => ReadAsync(CancellationToken.None);

        public Task<bool> ReadAsync(CancellationToken cancellationToken)
            => Task.FromResult(_incomeEnumerator.MoveNext());
    }
}