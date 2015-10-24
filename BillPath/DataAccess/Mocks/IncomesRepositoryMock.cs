using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Mocks
{
    public class IncomesRepositoryMock
        : Observable<IncomeRepositoryChange>, IIncomeRepository
    {
        private readonly IList<Income> _incomes;

        public IncomesRepositoryMock(IEnumerable<Income> incomes)
        {
            _incomes = new List<Income>(incomes ?? Enumerable.Empty<Income>());
        }
        public IncomesRepositoryMock(params Income[] incomes)
            : this(incomes.AsEnumerable())
        {
        }
        public IncomesRepositoryMock()
            : this(null)
        {
        }

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            => Task.FromResult(_incomes.Count);

        public IncomeReaderMock GetReader()
            => new IncomeReaderMock(_incomes.GetEnumerator());
        IItemReader<Income> IItemReaderProvider<Income>.GetReader()
            => GetReader();

        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            if (income == null)
                throw new ArgumentNullException(nameof(income));

            await Task.Yield();
            _incomes.Add(income);
            Notify(new IncomeRepositoryChange(income, IncomeRepositoryChangeAction.Add));
        }
    }
}