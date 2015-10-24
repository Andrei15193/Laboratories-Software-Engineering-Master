using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;

namespace BillPath.UserInterface.ViewModels
{
    public class PaginationViewModel<TItem>
        : ViewModel
    {
        private const int _itemsPerPage = 10;

        private abstract class State
        {
            public State(PaginationViewModel<TItem> context)
            {
                Context = context;
            }

            protected PaginationViewModel<TItem> Context
            {
                get;
            }

            public Task LoadAsync()
                => LoadAsync(CancellationToken.None);
            public async Task LoadAsync(CancellationToken cancellationToken)
            {
                var itemCount = await Context._itemReaderProvider.GetItemCountAsync(cancellationToken);
                var pageCount = (itemCount / _itemsPerPage);
                if (itemCount % _itemsPerPage > 0)
                    pageCount++;

                Context._state = new LoadedState(Context, pageCount);

                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.PageCount));
                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.Items));
            }

            public Task GoToPageAsync(int pageNumber)
                => GoToPageAsync(pageNumber, CancellationToken.None);
            public abstract Task GoToPageAsync(int pageNumber, CancellationToken cancellationToken);

            public abstract int PageCount
            {
                get;
            }
            public abstract int CurrentPage
            {
                get;
            }
            public abstract IEnumerable<TItem> Items
            {
                get;
            }
        }

        private sealed class InitialState
            : State
        {
            public InitialState(PaginationViewModel<TItem> context)
                : base(context)
            {
            }

            public override Task GoToPageAsync(int pageNumber, CancellationToken cancellationToken)
            {
                throw new InvalidOperationException();
            }

            public override int PageCount
            {
                get
                {
                    return 0;
                }
            }
            public override int CurrentPage
            {
                get
                {
                    return 0;
                }
            }
            public override IEnumerable<TItem> Items
            {
                get
                {
                    return null;
                }
            }
        }

        private class LoadedState
            : State
        {
            private int _pageCount;
            private int _currentPage;
            private IEnumerable<TItem> _items;

            public LoadedState(PaginationViewModel<TItem> context, int pageCount)
                : base(context)
            {
                _currentPage = 0;
                _pageCount = pageCount;
                _items = Enumerable.Empty<TItem>();
            }

            public override async Task GoToPageAsync(int pageNumber, CancellationToken cancellationToken)
            {
                if (pageNumber < 1 || PageCount < pageNumber)
                    throw new ArgumentException(nameof(pageNumber));

                var skippedItems = 0;
                var items = new List<TItem>();
                using (var itemsReader = Context._itemReaderProvider.GetReader())
                {
                    while (await itemsReader.ReadAsync(cancellationToken)
                        && skippedItems < (pageNumber - 1) * _itemsPerPage)
                        skippedItems++;

                    do
                        items.Add(itemsReader.Current);
                    while (await itemsReader.ReadAsync(cancellationToken)
                        && items.Count < _itemsPerPage);
                }

                _items = items;
                _currentPage = pageNumber;

                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.Items));
                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.CurrentPage));
            }

            public override int PageCount
            {
                get
                {
                    return _pageCount;
                }
            }
            public override int CurrentPage
            {
                get
                {
                    return _currentPage;
                }
            }
            public override IEnumerable<TItem> Items
            {
                get
                {
                    return _items;
                }
            }
        }

        private class ReloadPageState
            : State
        {
            public ReloadPageState(PaginationViewModel<TItem> context)
                : base(context)
            {
            }

            public override async Task GoToPageAsync(int pageNumber, CancellationToken cancellationToken)
            {
                await LoadAsync();
                await Context._state.GoToPageAsync(pageNumber);
            }

            public override int PageCount
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public override int CurrentPage
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public override IEnumerable<TItem> Items
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        private abstract class GoToAdjacentPageCommand
            : AsyncCommand
        {
            private readonly PaginationViewModel<TItem> _context;

            protected GoToAdjacentPageCommand(PaginationViewModel<TItem> context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                _context = context;
                _context.PropertyChanged +=
                    (sender, e) =>
                    {
                        if (nameof(PaginationViewModel<TItem>.PageCount).Equals(
                                e.PropertyName,
                                StringComparison.OrdinalIgnoreCase) ||
                            nameof(PaginationViewModel<TItem>.CurrentPage).Equals(
                                e.PropertyName,
                                StringComparison.OrdinalIgnoreCase))
                            RefreshCanExecute();
                    };

                RefreshCanExecute();
            }

            protected AsyncCommand<int> GoToPageCommand
                => _context.GoToPageCommand;
            protected int PageCount
                => _context.PageCount;
            protected int CurrentPage
                => _context.CurrentPage;

            protected abstract void RefreshCanExecute();
        }

        private sealed class GoToNextPageAsyncCommand
            : GoToAdjacentPageCommand
        {
            public GoToNextPageAsyncCommand(PaginationViewModel<TItem> context)
                : base(context)
            {
            }

            protected override Task OnExecuteAsync(object parameter)
            {
                return GoToPageCommand.ExecuteAsync(CurrentPage + 1);
            }

            protected override void RefreshCanExecute()
            {
                CanExecute = PageCount > CurrentPage;
            }
        }

        private sealed class GoToPreviousPageAsyncCommand
            : GoToAdjacentPageCommand
        {
            public GoToPreviousPageAsyncCommand(PaginationViewModel<TItem> context)
                : base(context)
            {
            }

            protected override Task OnExecuteAsync(object parameter)
            {
                return GoToPageCommand.ExecuteAsync(CurrentPage - 1);
            }

            protected override void RefreshCanExecute()
            {
                CanExecute = 1 < CurrentPage;
            }
        }

        private readonly IItemReaderProvider<TItem> _itemReaderProvider;
        private State _state;

        public PaginationViewModel(IItemReaderProvider<TItem> itemReaderProvider)
        {
            if (itemReaderProvider == null)
                throw new ArgumentNullException(nameof(itemReaderProvider));

            _itemReaderProvider = itemReaderProvider;
            _state = new InitialState(this);

            (_itemReaderProvider as IObservable<RepositoryChange<TItem>>)?.Subscribe(
                new DelegateObserver<RepositoryChange<TItem>>(
                    onNext: async change =>
                    {
                        if (_state is LoadedState)
                        {
                            var currentPage = CurrentPage;
                            _state = new ReloadPageState(this);
                            await GoToPageCommand.ExecuteAsync(currentPage == 0 ? 1 : currentPage);
                        }
                    }));

            LoadCommand = new DelegateAsyncCommand(
                (parameter, cancellationToken) => _state.LoadAsync(cancellationToken));
            GoToPageCommand = new DelegateAsyncCommand<int>(
                (pageNumber, cancellationToken) => _state.GoToPageAsync(pageNumber, cancellationToken));

            GoToNextPageCommand = new GoToNextPageAsyncCommand(this);
            GoToPreviousPageCommand = new GoToPreviousPageAsyncCommand(this);
        }

        public AsyncCommand LoadCommand
        {
            get;
        }


        public AsyncCommand<int> GoToPageCommand
        {
            get;
        }
        public AsyncCommand GoToNextPageCommand
        {
            get;
        }
        public AsyncCommand GoToPreviousPageCommand
        {
            get;
        }

        public int PageCount
        {
            get
            {
                return _state.PageCount;
            }
        }
        public int CurrentPage
        {
            get
            {
                return _state.CurrentPage;
            }
        }
        public IEnumerable<TItem> Items
        {
            get
            {
                return _state.Items;
            }
        }
    }
}