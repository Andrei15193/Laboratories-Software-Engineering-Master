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
        public async Task TestPageCountIs0WhenAccessedWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.AreEqual(
                0,
                viewModel.PageCount);
        }
        [TestMethod]
        public async Task TestCurrentPageIs0WhenAccessedWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.AreEqual(
                0,
                viewModel.CurrentPage);
        }
        [TestMethod]
        public async Task TestItemsIsNullWhenAccessedWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.IsNull(viewModel.Items);
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
        public async Task TestExceptionIsThrownWhenExecutingGoToNextPageCommandWithoutLoad()
        {
            _SkipLoad = true;
            var viewModel = await _GetViewModelAsync();
            Assert.IsInstanceOfType(
                await viewModel
                    .GoToNextPageCommand
                    .ExecuteAsync(null)
                    .ContinueWith(goToNextPageTask => goToNextPageTask.Exception.InnerException),
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
                2,
                _PropertyChanges.Count(propertyChange => nameof(PaginationViewModel<int>.Items).Equals(
                    propertyChange,
                    StringComparison.OrdinalIgnoreCase)));
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

            Assert.AreEqual(pageNumber, viewModel.CurrentPage);
            _AssertItems(viewModel, expectedItems);
        }

        [TestMethod]
        public async Task TestCannotExecuteGoToNextPageCommandWhenThereAreNoPages()
        {
            _ItemCount = 0;
            var viewModel = await _GetViewModelAsync();

            Assert.IsFalse(viewModel.GoToNextPageCommand.CanExecute);
        }
        [DataTestMethod]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(20)]
        [DataRow(25)]
        public async Task TestCannotExecuteGoToNextPageCommandWhenLastPageIsSelected(int itemsCount)
        {
            _ItemCount = itemsCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(viewModel.PageCount);

            Assert.IsFalse(viewModel.GoToNextPageCommand.CanExecute);
        }
        [TestMethod]
        public async Task TestPropertyChangedEventIsRaisedWhenGoToNextPageCommandIsExecuted()
        {
            _ItemCount = 15;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToNextPageCommand.ExecuteAsync(null);

            Assert.AreEqual(
                2,
                _PropertyChanges.Count(propertyChange => nameof(PaginationViewModel<int>.Items).Equals(
                    propertyChange,
                    StringComparison.OrdinalIgnoreCase)));
        }
        [TestMethod]
        [DataRow(15, 0, 10)]
        [DataRow(20, 0, 10)]
        [DataRow(25, 0, 10)]
        public async Task TestItemsAreLoadedFromFirstPageWhenGoToNextPageIsCalledFirst(int itemCount, int pageItemStart, int pageItemCount)
        {
            _ItemCount = itemCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToNextPageCommand.ExecuteAsync(null);

            var expectedItems = Enumerable.Range(
                pageItemStart,
                pageItemCount);

            Assert.AreEqual(1, viewModel.CurrentPage);
            _AssertItems(viewModel, expectedItems);
        }
        [TestMethod]
        [DataRow(15, 1, 10, 5)]
        [DataRow(20, 1, 10, 10)]
        [DataRow(25, 1, 10, 10)]
        [DataRow(25, 2, 20, 5)]
        public async Task TestItemsAreLoadedFromNextPageWhenGoToPageCommandWasExecutedPreviously(int itemCount, int initialPage, int pageItemStart, int pageItemCount)
        {
            _ItemCount = itemCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(initialPage);
            await viewModel.GoToNextPageCommand.ExecuteAsync(null);

            var expectedItems = Enumerable.Range(
                pageItemStart,
                pageItemCount);

            Assert.AreEqual(initialPage + 1, viewModel.CurrentPage);
            _AssertItems(viewModel, expectedItems);
        }

        [TestMethod]
        public async Task TestCannotExecuteGoToPreviousPageCommandWhenThereAreNoPages()
        {
            _ItemCount = 0;
            var viewModel = await _GetViewModelAsync();

            Assert.IsFalse(viewModel.GoToPreviousPageCommand.CanExecute);
        }
        [DataTestMethod]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(20)]
        [DataRow(25)]
        public async Task TestCannotExecuteGoToPreviousPageCommandWhenFirstPageIsSelected(int itemsCount)
        {
            _ItemCount = itemsCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(1);

            Assert.IsFalse(viewModel.GoToPreviousPageCommand.CanExecute);
        }
        [TestMethod]
        public async Task TestPropertyChangedEventIsRaisedWhenGoToPreviousPageCommandIsExecuted()
        {
            _ItemCount = 15;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(2);
            await viewModel.GoToPreviousPageCommand.ExecuteAsync(null);

            Assert.AreEqual(
                3,
                _PropertyChanges.Count(propertyChange => nameof(PaginationViewModel<int>.Items).Equals(
                    propertyChange,
                    StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        [DataRow(15, 2, 0, 10)]
        [DataRow(20, 2, 0, 10)]
        [DataRow(25, 2, 0, 10)]
        [DataRow(25, 3, 10, 10)]
        public async Task TestItemsAreLoadedFromPreviousPageWhenGoToPageCommandWasExecutedPreviously(int itemCount, int initialPage, int pageItemStart, int pageItemCount)
        {
            _ItemCount = itemCount;
            var viewModel = await _GetViewModelAsync();
            await viewModel.GoToPageCommand.ExecuteAsync(initialPage);
            await viewModel.GoToPreviousPageCommand.ExecuteAsync(null);

            var expectedItems = Enumerable.Range(
                pageItemStart,
                pageItemCount);

            Assert.AreEqual(initialPage - 1, viewModel.CurrentPage);
            _AssertItems(viewModel, expectedItems);
        }

        private static void _AssertItems(PaginationViewModel<int> viewModel, IEnumerable<int> expectedItems)
        {
            Assert.IsTrue(
                expectedItems.SequenceEqual(viewModel.Items),
                string.Format(
                    "Expected {{{0}}} but instead the page contains {{{1}}}",
                    string.Join(", ", expectedItems),
                    string.Join(", ", viewModel.Items)));
        }
    }
}