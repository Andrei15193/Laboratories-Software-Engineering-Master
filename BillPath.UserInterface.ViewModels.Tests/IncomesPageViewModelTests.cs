using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.DataAccess.Xml;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class IncomesPageViewModelTests
    {
        private IncomeXmlMockRepository _repository;
        private IncomeObservableRepository _observableRepository;
        private IncomesPageViewModel _viewModel;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _repository = new IncomeXmlMockRepository();
            _observableRepository = new IncomeObservableRepository(_repository);
            _viewModel = new IncomesPageViewModel(_observableRepository);

            await _WaitLoadViewModelAsync();
        }

        private async Task _WaitLoadViewModelAsync()
        {
            while (_viewModel.Loading)
                await Task.Delay(10);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _viewModel = null;
            _observableRepository = null;
            _repository.Dispose();
            _repository = null;
        }

        [TestMethod]
        public void TestPagedViewModelOverEmptyRepositoryLoadsFirstPageWithNoItems()
            => Assert.IsFalse(_viewModel.Items.Any());

        [TestMethod]
        public async Task TestPagedViewModelOverRepositoryWithOneItemsLoadsItOnFirstPage()
        {
            var expectedIncome = new Income();

            await _observableRepository.SaveAsync(expectedIncome);
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(1, _viewModel.Items.Count());

            var actualIncome = _viewModel.Items.First();
            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(
                expectedIncome,
                (Income)actualIncome.ModelState.Model));
        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(10, 1)]
        [DataRow(11, 2)]
        public async Task TestCreatingAViewModelLoadsPageCount(int incomesCount, int expectedPageCount)
        {
            foreach (var incomeIndex in Enumerable.Range(0, incomesCount))
                await _observableRepository.SaveAsync(new Income());
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(expectedPageCount, _viewModel.PagesCount);
        }

        [DataTestMethod]
        [DataRow(1, 1, 0, 1)]
        [DataRow(10, 1, 0, 10)]
        [DataRow(11, 1, 0, 10)]
        [DataRow(11, 2, 10, 1)]
        public async Task TestLoadPage(int totalIncomesCount, int page, int incomeIndexStartOnPage, int incomesCountOnPage)
        {
            foreach (var incomeIndex in Enumerable.Range(0, totalIncomesCount))
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Description = incomeIndex.ToString(),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5)).AddMinutes(-incomeIndex)
                    });

            await _WaitLoadViewModelAsync();

            _viewModel.GoToPageCommand.PageNumber = page;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            var missingIncomeIndexesOnPage = _GetMissingIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);
            var extraIncomeIndexesOnPage = _GetExtraIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);

            Assert.IsFalse(
                missingIncomeIndexesOnPage.Any(),
                $"Expected but missing incomes: {string.Join(", ", missingIncomeIndexesOnPage)}");
            Assert.IsFalse(
                extraIncomeIndexesOnPage.Any(),
                $"Unexpected incomes: {string.Join(", ", extraIncomeIndexesOnPage)}");
        }
        private IEnumerable<int> _GetMissingIncomeIndexesOnPage(int incomeIndexStartOnPage, int incomesCountOnPage)
            => from incomeIndex in Enumerable.Range(incomeIndexStartOnPage, incomesCountOnPage)
               join income in from incomeViewModel in _viewModel.Items
                              select (Income)incomeViewModel.ModelState.Model
               on incomeIndex.ToString() equals income.Description
               into incomesByIndexes
               where !incomesByIndexes.Any()
               select incomeIndex;
        private IEnumerable<int> _GetExtraIncomeIndexesOnPage(int incomeIndexStartOnPage, int incomesCountOnPage)
            => from incomeViewModel in _viewModel.Items
               let income = (Income)incomeViewModel.ModelState.Model
               join incomeIndex in Enumerable.Range(incomeIndexStartOnPage, incomesCountOnPage)
               on income.Description equals incomeIndex.ToString()
               into indexesByIncomeViewModel
               where !indexesByIncomeViewModel.Any()
               select int.Parse(income.Description);

        [TestMethod]
        public async Task TestSelectingAPageRaisesPropertyChangedForLoading()
        {
            var propertyChanges = new List<string>();

            await _observableRepository.SaveAsync(new Income());
            await _WaitLoadViewModelAsync();

            _viewModel.PropertyChanged +=
                (sender, e) => propertyChanges.Add(e.PropertyName);

            _viewModel.GoToPageCommand.PageNumber = 1;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(2, propertyChanges.Count(nameof(IncomesPageViewModel.Loading).Equals));
        }

        [TestMethod]
        public async Task TestSelectingAPageRaisesPropertyChangedForItems()
        {
            var propertyChanges = new List<string>();

            await _observableRepository.SaveAsync(new Income());
            await _WaitLoadViewModelAsync();

            _viewModel.PropertyChanged +=
                (sender, e) => propertyChanges.Add(e.PropertyName);

            _viewModel.GoToPageCommand.PageNumber = 1;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(1, propertyChanges.Count(nameof(IncomesPageViewModel.Items).Equals));
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(10, 1)]
        [DataRow(11, 1)]
        [DataRow(11, 2)]
        public async Task TestSelectedPageIsProvidedThroughCorrespondingProperty(int totalIncomesCount, int page)
        {
            foreach (var incomeIndex in Enumerable.Range(0, totalIncomesCount))
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Description = incomeIndex.ToString(),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5)).AddMinutes(-incomeIndex)
                    });
            await _WaitLoadViewModelAsync();

            _viewModel.GoToPageCommand.PageNumber = page;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(page, _viewModel.SelectedPage);
        }

        [TestMethod]
        public async Task TestSavingIncomeToRepositoryUpdatesViewModel()
        {
            var expectedIncome =
                new Income
                {
                    Description = "0",
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5))
                };

            foreach (var incomeIndex in Enumerable.Range(1, 3))
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Description = incomeIndex.ToString(),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5)).AddMinutes(-incomeIndex)
                    });
            await _WaitLoadViewModelAsync();

            await _observableRepository.SaveAsync(expectedIncome);
            await _WaitLoadViewModelAsync();

            Assert.AreEqual(4, _viewModel.Items.Count());

            var actualIncome = (Income)_viewModel.Items.First().ModelState.Model;
            Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
        }

        [TestMethod]
        public async Task TestSavingIncomeThatCreatesANewPageRaisesPropertyChangedForPagesCount()
        {
            var propertyChanges = new List<string>();

            foreach (var incomeIndex in Enumerable.Range(0, 10))
                await _observableRepository.SaveAsync(new Income());
            await _WaitLoadViewModelAsync();

            _viewModel.PropertyChanged += (sender, e) => propertyChanges.Add(e.PropertyName);
            await _WaitLoadViewModelAsync();

            await _observableRepository.SaveAsync(new Income());
            while (_viewModel.Loading)
                await Task.Delay(10);

            Assert.AreEqual(1, propertyChanges.Count(nameof(IncomesPageViewModel.PagesCount).Equals));
        }

        [DataTestMethod]
        [DataRow(11, 1, 10, 1)]
        [DataRow(20, 1, 10, 10)]
        [DataRow(21, 2, 20, 1)]
        public async Task TestExecutingGoToNextPageCommandLoadsItemsFromNextPage(int totalIncomesCount, int initialPage, int incomeIndexStartOnPage, int incomesCountOnPage)
        {
            foreach (var incomeIndex in Enumerable.Range(0, totalIncomesCount))
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Description = incomeIndex.ToString(),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5)).AddMinutes(-incomeIndex)
                    });

            await _WaitLoadViewModelAsync();

            _viewModel.GoToPageCommand.PageNumber = initialPage;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();
            await _viewModel.GoToNextPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            var missingIncomeIndexesOnPage = _GetMissingIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);
            var extraIncomeIndexesOnPage = _GetExtraIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);

            Assert.IsFalse(
                missingIncomeIndexesOnPage.Any(),
                $"Expected but missing incomes: {string.Join(", ", missingIncomeIndexesOnPage)}");
            Assert.IsFalse(
                extraIncomeIndexesOnPage.Any(),
                $"Unexpected incomes: {string.Join(", ", extraIncomeIndexesOnPage)}");
        }
        [DataTestMethod]
        [DataRow(11, 2, 0, 10)]
        [DataRow(20, 2, 0, 10)]
        [DataRow(21, 3, 10, 10)]
        public async Task TestExecutingGoToPreviousPageCommandLoadsItemsFromPreviousPage(int totalIncomesCount, int initialPage, int incomeIndexStartOnPage, int incomesCountOnPage)
        {
            foreach (var incomeIndex in Enumerable.Range(0, totalIncomesCount))
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Description = incomeIndex.ToString(),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5)).AddMinutes(-incomeIndex)
                    });

            await _WaitLoadViewModelAsync();

            _viewModel.GoToPageCommand.PageNumber = initialPage;
            await _viewModel.GoToPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();
            await _viewModel.GoToPreviousPageCommand.ExecuteAsync(null);
            await _WaitLoadViewModelAsync();

            var missingIncomeIndexesOnPage = _GetMissingIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);
            var extraIncomeIndexesOnPage = _GetExtraIncomeIndexesOnPage(
                incomeIndexStartOnPage,
                incomesCountOnPage);

            Assert.IsFalse(
                missingIncomeIndexesOnPage.Any(),
                $"Expected but missing incomes: {string.Join(", ", missingIncomeIndexesOnPage)}");
            Assert.IsFalse(
                extraIncomeIndexesOnPage.Any(),
                $"Unexpected incomes: {string.Join(", ", extraIncomeIndexesOnPage)}");
        }

        [DataTestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(11, 0, 1)]
        [DataRow(11, 5, 1)]
        [DataRow(11, 10, 1)]
        [DataRow(21, 0, 2)]
        [DataRow(21, 1, 2)]
        [DataRow(21, 2, 2)]
        [DataRow(21, 3, 2)]
        [DataRow(21, 5, 2)]
        [DataRow(21, 8, 2)]
        [DataRow(21, 13, 2)]
        [DataRow(21, 20, 2)]
        public async Task TestRemovingIncomeWhichRemovesAPageUpdatesPagesCount(int totalIncomeCount, int indexToRemove, int expectedPageCount)
        {
            var incomeToRemove =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                    Description = "Test description " + indexToRemove.ToString()
                };

            for (var incomeIndex = 0; incomeIndex < totalIncomeCount; incomeIndex++)
                await _observableRepository.SaveAsync(
                    new Income
                    {
                        Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                        Description = "Test description " + incomeIndex.ToString()
                    });

            await _observableRepository.RemoveAsync(incomeToRemove);

            Assert.AreEqual(expectedPageCount, _viewModel.PagesCount);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        public async Task TestLoadedIncomesOntoPageCanBeRemoved(int totalIncomeCount)
        {
            for (var incomeIndex = 0; incomeIndex < totalIncomeCount; incomeIndex++)
                await _observableRepository.SaveAsync(new Income());

            await _WaitLoadViewModelAsync();

            foreach (var incomeViewModel in _viewModel.Items)
                Assert.IsTrue(incomeViewModel.RemoveCommand.CanExecute);
        }
    }
}