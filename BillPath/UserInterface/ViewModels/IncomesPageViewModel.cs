using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;

namespace BillPath.UserInterface.ViewModels
{
    public class IncomesPageViewModel
        : INotifyPropertyChanged
    {
        private const int _itemsPerPage = 10;

        private bool _loading;
        private int _selectedPage;
        private int _pagesCount;
        private IEnumerable<IncomeViewModel> _items;
        private readonly TaskScheduler _taskScheduler;
        private readonly IIncomeXmlRepository _repository;

        public IncomesPageViewModel(IncomeXmlObservableRepository repository)
        {
            _repository = repository;
            _taskScheduler = TaskScheduler.Current;

            GoToPageCommand = new GoToPageAsyncCommand(this);
            GoToNextPageCommand = _GetGoToNextPageCommand();
            GoToPreviousPageCommand = _GetGoToPreviousPageCommand();

            repository.SavedIncome += async delegate { await _LoadAsync(SelectedPage, CancellationToken.None); };
            repository.RemovedIncome += async delegate { await _LoadAsync(SelectedPage, CancellationToken.None); };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _LoadAsync(1, CancellationToken.None);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private AsyncCommand _GetGoToNextPageCommand()
        {
            var command = new DelegateAsyncCommand(
                (parameter, cancellationToken) => _LoadAsync(SelectedPage + 1, cancellationToken));
            PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(SelectedPage).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase)
                        || nameof(PagesCount).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                        command.CanExecute = SelectedPage < PagesCount;
                };

            return command;
        }
        private AsyncCommand _GetGoToPreviousPageCommand()
        {
            var command = new DelegateAsyncCommand(
                (parameter, cancellationToken) => _LoadAsync(SelectedPage - 1, cancellationToken));
            PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(SelectedPage).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase)
                        || nameof(PagesCount).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                        command.CanExecute = SelectedPage > 1;
                };

            return command;
        }

        private async Task _LoadAsync(int pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                Loading = true;
                var incomes = new List<IncomeViewModel>();

                using (var reader = await _repository.GetReaderAsync(cancellationToken))
                {
                    await reader.SkipAsync(_itemsPerPage * (pageNumber - 1), cancellationToken);

                    while (incomes.Count < _itemsPerPage && await reader.ReadAsync(cancellationToken))
                        incomes.Add(new IncomeViewModel(_repository, reader.Current));
                }

                var totalIncomes = await _repository.GetCountAsync(cancellationToken);
                if (totalIncomes == 0)
                    PagesCount = 0;
                else
                {
                    var pagesCount = totalIncomes / _itemsPerPage;
                    if (totalIncomes % _itemsPerPage > 0)
                        pagesCount += 1;

                    PagesCount = pagesCount;
                }
                Items = incomes;
                GoToPageCommand.PageNumber = SelectedPage = pageNumber;
            }
            finally
            {
                Loading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
            => PropertyChanged?.Invoke(this, propertyChangedEventArgs);

        public bool Loading
        {
            get
            {
                return _loading;
            }
            private set
            {
                _loading = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Loading)));
            }
        }

        public int SelectedPage
        {
            get
            {
                return _selectedPage;
            }
            private set
            {
                _selectedPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedPage)));
            }
        }
        public int PagesCount
        {
            get
            {
                return _pagesCount;
            }
            private set
            {
                _pagesCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PagesCount)));
            }
        }

        public IEnumerable<IncomeViewModel> Items
        {
            get
            {
                return _items;
            }
            private set
            {
                _items = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        public GoToPageAsyncCommand GoToPageCommand { get; }
        public AsyncCommand GoToNextPageCommand { get; }
        public AsyncCommand GoToPreviousPageCommand { get; }

        public sealed class GoToPageAsyncCommand
            : AsyncCommand
        {
            private int _pageNumber;
            private readonly IncomesPageViewModel _viewModel;

            public int? PageNumber
            {
                get
                {
                    return _pageNumber;
                }
                set
                {
                    _pageNumber = value ?? 0;
                    CanExecute = (1 <= _pageNumber && _pageNumber <= _viewModel.PagesCount);
                }
            }

            public GoToPageAsyncCommand(IncomesPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            protected override Task OnExecuteAsync(object parameter)
                => _viewModel._LoadAsync(_pageNumber, CancellationToken);
        }
    }
}