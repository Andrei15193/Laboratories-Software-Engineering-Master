using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.DataAccess.Mocks;
using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    internal class IncomesRepository
        : IIncomesRepository
    {
        private readonly IIncomesRepository _incomesRepository;

        private sealed class IncomeReaderAdapter
            : IItemReader<Income>
        {
            private readonly IItemReader<Income> _reader;
            private readonly int _millisecondsDelay;

            public IncomeReaderAdapter(IItemReader<Income> reader, int millisecondsDelay)
            {
                _reader = reader;
                _millisecondsDelay = millisecondsDelay;
            }

            public Income Current
            {
                get
                {
                    return _reader.Current;
                }
            }

            public void Dispose()
            {
                _reader.Dispose();
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);

            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
            {
                await Task.Delay(_millisecondsDelay, cancellationToken);
                return await _reader.ReadAsync(cancellationToken);
            }
        }

        public IncomesRepository()
        {
            _incomesRepository = new IncomesRepositoryMock(
                 Enumerable.Repeat(
                     new Income
                     {
                         Amount = new Amount(10.2m, new Currency(new RegionInfo("RO"))),
                         DateRealized = DateTimeOffset.Now,
                         Description = "Test description"
                     },
                     22));
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

        public Task SaveAsync(Income income)
            => SaveAsync(income, CancellationToken.None);
        public async Task SaveAsync(Income income, CancellationToken cancellationToken)
        {
            await _incomesRepository.SaveAsync(income);
            await Task.Delay(MillisecondsDelay, cancellationToken);
        }

        public IItemReader<Income> GetReader()
            => new IncomeReaderAdapter(_incomesRepository.GetReader(), ReaderMillisecondsDelay);

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public async Task<int> GetItemCountAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay, cancellationToken);
            return await _incomesRepository.GetItemCountAsync(cancellationToken);
        }

        public IDisposable Subscribe(IObserver<RepositoryChange<Income>> observer)
        {
            return _incomesRepository.Subscribe(observer);
        }
    }
}