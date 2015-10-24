using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    internal class IncomeRepository
        : DataAccess.IncomeRepository
    {
        private readonly IList<Income> _incomes = new List<Income>(
            Enumerable.Repeat(
                new Income
                {
                    Amount = new Amount(10.2m, new Currency(new RegionInfo("RO"))),
                    DateRealized = DateTimeOffset.Now,
                    Description = "Test description"
                },
                22));

        private sealed class IncomeReader
            : IItemReader<Income>
        {
            private readonly int _millisecondsDelay;
            private readonly IEnumerator<Income> _income;

            public IncomeReader(IncomeRepository repository)
            {
                if (repository == null)
                    throw new ArgumentNullException(nameof(repository));

                _millisecondsDelay = repository.ReaderMillisecondsDelay;
                _income = repository._incomes.GetEnumerator();
            }

            public Income Current
            {
                get
                {
                    return _income.Current;
                }
            }

            public void Dispose()
            {
                _income.Dispose();
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);

            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(_millisecondsDelay, cancellationToken);
                return _income.MoveNext();
            }
        }

        public int MillisecondsDelay
        {
            get;
            set;
        }
        public int ReaderMillisecondsDelay
        {
            get;
            set;
        }

        public IList<Income> Incomes
        {
            get
            {
                return _incomes;
            }
        }

        public override async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay);
            Incomes.Add(income);
        }

        public override IItemReader<Income> GetReader()
        {
            return new IncomeReader(this);
        }

        public override async Task<int> GetItemCountAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay);
            return _incomes.Count;
        }
    }
}
