using System;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomeViewModelReaderProvider
        : IObservable<RepositoryChange<IncomeViewModel>>, IItemReaderProvider<IncomeViewModel>
    {
        private readonly IIncomesRepository _repository;

        private sealed class IncomeViewModelReader
            : IItemReader<IncomeViewModel>
        {
            private IncomeViewModel _current;
            private readonly IIncomesRepository _repository;
            private readonly IItemReader<Income> _incomeReader;

            public IncomeViewModelReader(IIncomesRepository repository)
            {
                if (repository == null)
                    throw new ArgumentNullException(nameof(repository));

                _current = null;
                _repository = repository;
                _incomeReader = repository.GetReader();
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
                    _current = new IncomeViewModel(_repository) { ModelState = ModelState.GetFor(_incomeReader.Current) };
                    return true;
                }
                else
                {
                    _current = null;
                    return false;
                }
            }
        }

        public IncomeViewModelReaderProvider(IIncomesRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public Task<int> GetItemCountAsync()
            => GetItemCountAsync(CancellationToken.None);
        public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            => _repository.GetItemCountAsync(cancellationToken);

        public IItemReader<IncomeViewModel> GetReader()
            => new IncomeViewModelReader(_repository);

        public IDisposable Subscribe(IObserver<RepositoryChange<IncomeViewModel>> observer)
        {
            return (_repository as IObservable<RepositoryChange<Income>>)?.Subscribe(
                new DelegateObserver<RepositoryChange<Income>>(
                    onNext: change => observer.OnNext(new RepositoryChange<IncomeViewModel>(
                        new IncomeViewModel(_repository)
                        {
                            ModelState = ModelState.GetFor(change.Item)
                        },
                        change.Action)),
                    onError: observer.OnError,
                    onCompleted: observer.OnCompleted));
        }
    }
}