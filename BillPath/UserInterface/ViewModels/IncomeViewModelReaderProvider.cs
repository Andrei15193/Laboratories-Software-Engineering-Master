using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomeViewModelReaderProvider
        : IItemReaderProvider<IncomeViewModel>
    {
        private readonly IItemReaderProvider<Income> _incomeReaderProvider;

        private sealed class IncomeViewModelReader
            : IItemReader<IncomeViewModel>
        {
            private IncomeViewModel _current;
            private readonly IItemReader<Income> _incomeReader;

            public IncomeViewModelReader(IItemReader<Income> incomeReader)
            {
                if (incomeReader == null)
                    throw new ArgumentNullException(nameof(incomeReader));

                _current = null;
                _incomeReader = incomeReader;
            }

            public IncomeViewModel Current
            {
                get
                {
                    if (_current == null)
                        throw new InvalidOperationException("There either are no more elements to read or ReadAsync has never been called.");

                    return _current;
                }
            }

            public void Dispose()
            {
                _incomeReader.Dispose();
            }

            public Task<bool> ReadAsync()
                => ReadAsync(CancellationToken.None);

            public async Task<bool> ReadAsync(CancellationToken cancellationToken)
            {
                if (await _incomeReader.ReadAsync(cancellationToken))
                {
                    _current = new IncomeViewModel(_incomeReader.Current);
                    return true;
                }
                else
                {
                    _current = null;
                    return false;
                }
            }
        }

        public IncomeViewModelReaderProvider(IItemReaderProvider<Income> incomeReaderProvider)
        {
            if (incomeReaderProvider == null)
                throw new ArgumentNullException(nameof(incomeReaderProvider));

            _incomeReaderProvider = incomeReaderProvider;
        }

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            => _incomeReaderProvider.GetItemCountAsync(cancellationToken);

        public IItemReader<IncomeViewModel> GetReader()
            => new IncomeViewModelReader(_incomeReaderProvider.GetReader());
    }
}