using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class PaginationViewModelTests
    {
        private sealed class ItemReaderProviderCollectionMock<TItem>
            : IItemReaderProvider<TItem>
        {
            private readonly IEnumerable<TItem> _items;

            public ItemReaderProviderCollectionMock(IEnumerable<TItem> items)
            {
                if (items == null)
                    throw new ArgumentNullException(nameof(items));

                _items = items;
            }

            public Task<int> GetItemCountAsync()
                => GetItemCountAsync(CancellationToken.None);
            public Task<int> GetItemCountAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_items.Count());
            }

            public IItemReader<TItem> GetReader()
            {
                return new ItemReaderCollectionMock(_items.GetEnumerator());
            }

            private sealed class ItemReaderCollectionMock
                : IItemReader<TItem>
            {
                private readonly IEnumerator<TItem> _enumerator;

                public ItemReaderCollectionMock(IEnumerator<TItem> enumerator)
                {
                    if (enumerator == null)
                        throw new ArgumentNullException(nameof(enumerator));

                    _enumerator = enumerator;
                }

                public TItem Current
                    => _enumerator.Current;

                public Task<bool> ReadAsync()
                    => ReadAsync(CancellationToken.None);
                public Task<bool> ReadAsync(CancellationToken cancellationToken)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return Task.FromResult(_enumerator.MoveNext());
                }

                public void Dispose()
                {
                    _enumerator.Dispose();
                }
            }
        }

        private PaginationViewModel<int> _viewModel;

        private int _ItemCount
        {
            get;
            set;
        }
        private bool _SkipLoad
        {
            get;
            set;
        }

        private async Task<PaginationViewModel<int>> _GetViewModelAsync()
        {
            if (_viewModel == null)
                _viewModel = await _CreateViewModelAsync();

            return _viewModel;
        }
        private async Task<PaginationViewModel<int>> _CreateViewModelAsync()
        {
            var propertyChanges = new List<string>();

            _PropertyChanges = propertyChanges;
            var viewModel = new PaginationViewModel<int>(
                new ItemReaderProviderCollectionMock<int>(
                    Enumerable.Range(
                        0,
                        _ItemCount)));
            viewModel.PropertyChanged += (sender, e) => propertyChanges.Add(e.PropertyName);

            if (!_SkipLoad)
                await viewModel.LoadCommand.ExecuteAsync(null);

            return viewModel;
        }

        private IReadOnlyList<string> _PropertyChanges
        {
            get;
            set;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _viewModel = null;
            _PropertyChanges = null;
        }

        [TestMethod]
        public async Task TestExceptionIsThrownIfPageCountIsAccessedWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.ThrowsException<InvalidOperationException>(() => viewModel.PageCount);
        }
        [TestMethod]
        public async Task TestExceptionIsThrownIfPageItemsAreAccessedWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.ThrowsException<InvalidOperationException>(() => viewModel.Items);
        }
        [TestMethod]
        public async Task TestExceptionIsThrownWhenExecutingGoToPageCommandWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.IsInstanceOfType(
                await viewModel
                    .GoToPageCommand
                    .ExecuteAsync(1)
                    .ContinueWith(goToPageTask => goToPageTask.Exception.InnerException),
                typeof(InvalidOperationException));
        }

        [TestMethod]
        public async Task TestPropertyChangedIsCalledAccordinglyAfterExecutingLoad()
        {
            await _GetViewModelAsync();
            Assert.AreEqual(
                1,
                _PropertyChanges
                    .Count(propertyChange => nameof(PaginationViewModel<int>.PageCount).Equals(
                        propertyChange,
                        StringComparison.OrdinalIgnoreCase)));
        }
        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(5, 1)]
        [DataRow(10, 1)]
        [DataRow(15, 2)]
        [DataRow(20, 2)]
        public async Task TestPageCountIsSetAccordinglyAfterLoad(int itemCount, int pageCount)
        {
            _ItemCount = itemCount;

            Assert.AreEqual(
                pageCount,
                (await _GetViewModelAsync()).PageCount);
        }

        [TestMethod]
        public async Task TestPropertyChangedEventIsRaisedWhenGoToPageCommandIsExecuted()
        {
            _ItemCount = 5;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(1);

            Assert.AreEqual(
                nameof(PaginationViewModel<int>.Items),
                _PropertyChanges.Last());
        }
        [TestMethod]
        public async Task TestExceptionIsThrownWhenExecutingGoToPageWhenPageCountIs0()
        {
            _ItemCount = 0;
            var viewModel = await _GetViewModelAsync();

            Assert.IsInstanceOfType(
                await viewModel
                    .GoToPageCommand
                    .ExecuteAsync(1)
                    .ContinueWith(goToPageCommandTask => goToPageCommandTask.Exception.InnerException),
                typeof(ArgumentException));
        }
        [DataTestMethod]
        [DataRow(5, 2)]
        [DataRow(5, 0)]
        [DataRow(5, -1)]
        [DataRow(10, 2)]
        [DataRow(10, 0)]
        [DataRow(10, -1)]
        [DataRow(15, 3)]
        [DataRow(15, 0)]
        [DataRow(15, -1)]
        public async Task TestExceptionIsThrownWhenExecutingGoToPageWhenPageNumberIsNotBetween1AndPageCount(int itemCount, int pageNumber)
        {
            _ItemCount = itemCount;
            var viewModel = await _GetViewModelAsync();

            Assert.IsInstanceOfType(
                await viewModel
                    .GoToPageCommand
                    .ExecuteAsync(pageNumber)
                    .ContinueWith(goToPageCommandTask => goToPageCommandTask.Exception.InnerException),
                typeof(ArgumentException));
        }

        [DataTestMethod]
        [DataRow(5, 1, 0, 5)]
        [DataRow(15, 1, 0, 10)]
        [DataRow(15, 2, 10, 5)]
        [DataRow(20, 2, 10, 10)]
        [DataRow(25, 3, 20, 5)]
        public async Task TestItemsAreLoadedAccordingToNavigatedPage(int itemCount, int pageNumber, int pageItemStart, int pageItemCount)
        {
            _ItemCount = itemCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(pageNumber);

            var expectedItems = Enumerable.Range(
                pageItemStart,
                pageItemCount);

            Assert.IsTrue(
                expectedItems.SequenceEqual(viewModel.Items),
                string.Format(
                    "Expected {{{0}}} but instead the page contains {{{0}}}",
                    string.Join(", ", expectedItems),
                    string.Join(", ", viewModel.Items)));
        }
    }
}