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
                LoadCommand = new DelegateAsyncCommand(_LoadAsync);
            }

            public abstract AsyncCommand<int> GoToPageCommand
            {
                get;
            }

            public abstract IEnumerable<TItem> Items
            {
                get;
            }

            public AsyncCommand LoadCommand
            {
                get;
            }

            public abstract int PageCount
            {
                get;
            }

            protected PaginationViewModel<TItem> Context
            {
                get;
            }

            private async Task _LoadAsync(object parameter, CancellationToken cancellationToken)
            {
                var itemCount = await Context._itemReaderProvider.GetItemCountAsync(cancellationToken);
                var pageCount = (itemCount / _itemsPerPage);
                if (itemCount % _itemsPerPage > 0)
                    pageCount++;

                Context._state = new LoadedState(Context, pageCount);
            }
        }

        private sealed class InitialState
            : State
        {
            public InitialState(PaginationViewModel<TItem> context)
                : base(context)
            {
                GoToPageCommand = new DelegateAsyncCommand<int>(
                    delegate
                    {
                        throw new InvalidOperationException();
                    });
            }

            public override AsyncCommand<int> GoToPageCommand
            {
                get;
            }

            public override IEnumerable<TItem> Items
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }

            public override int PageCount
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private sealed class LoadedState
            : State
        {
            private int _pageCount;
            private IEnumerable<TItem> _items;

            public LoadedState(PaginationViewModel<TItem> context, int pageCount)
                : base(context)
            {
                _pageCount = pageCount;
                _items = Enumerable.Empty<TItem>();
                GoToPageCommand = new DelegateAsyncCommand<int>(_GoToPageAsync);

                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.PageCount));
                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.Items));
                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.GoToPageCommand));
            }

            public override AsyncCommand<int> GoToPageCommand
            {
                get;
            }

            public override IEnumerable<TItem> Items
            {
                get
                {
                    return _items;
                }
            }

            public override int PageCount
            {
                get
                {
                    return _pageCount;
                }
            }

            private async Task _GoToPageAsync(int pageNumber, CancellationToken cancellationToken)
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
                Context.OnPropertyChanged(nameof(PaginationViewModel<TItem>.Items));
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
        }

        public AsyncCommand<int> GoToPageCommand
        {
            get
            {
                return _state.GoToPageCommand;
            }
        }

        public IEnumerable<TItem> Items
        {
            get
            {
                return _state.Items;
            }
        }

        public AsyncCommand LoadCommand
        {
            get
            {
                return _state.LoadCommand;
            }
        }

        public int PageCount
        {
            get
            {
                return _state.PageCount;
            }
        }
    }
}