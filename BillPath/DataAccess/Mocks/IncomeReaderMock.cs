using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Mocks
{
    public class IncomeReaderMock
        : IItemReader<Income>
    {
        private readonly IEnumerator<Income> _incomeEnumerator;

        public IncomeReaderMock(IEnumerator<Income> incomeEnumerator)
        {
            if (incomeEnumerator == null)
                throw new ArgumentNullException(nameof(incomeEnumerator));

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