using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels.Tests
{
    internal sealed class IncomeReaderProviderMock
        : IItemReaderProvider<Income>
    {
        private readonly IEnumerable<Income> _incomes;

        public IncomeReaderProviderMock(IEnumerable<Income> incomes)
        {
            _incomes = incomes;
        }
        public IncomeReaderProviderMock(params Income[] incomes)
            : this(incomes.AsEnumerable())
        {
        }

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);

        public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            => Task.FromResult(_incomes.Count());

        public IItemReader<Income> GetReader()
            => new IncomeReaderMock(_incomes.GetEnumerator());
    }
}