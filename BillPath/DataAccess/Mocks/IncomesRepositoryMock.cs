using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Mocks
{
    public class IncomesRepositoryMock
        : Observable<RepositoryChange<Income>>, IIncomesRepository
    {
        private readonly List<Income> _incomes;

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

            var insertIndex = _incomes.FindIndex(existingIncome => income.DateRealized > existingIncome.DateRealized);
            if (insertIndex == -1)
                _incomes.Insert(0, income);
            else
                _incomes.Insert(insertIndex, income);
            Notify(new RepositoryChange<Income>(income, RepositoryChangeAction.Add));
        }
    }
}