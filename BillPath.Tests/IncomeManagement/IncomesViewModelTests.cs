using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BillPath.Tests.IncomeManagement
{
    [TestClass]
    public class IncomesViewModelTests
    {
        [TestMethod]
        public async Task TestAddIncomeThroughAddCommand()
        {
            Income savedIncome = null;
            var income = new Income
            {
                Amount = 10.1m,
                Currency = new Currency(new RegionInfo("RO")),
                DateRealized = DateTimeOffset.Now,
                Description = "test description"
            };
            var repositoryMock = new Mock<IncomeRepository>();
            repositoryMock
                .Setup(repository =>
                    repository.SaveAsync(It.IsAny<Income>(), It.IsAny<CancellationToken>()))
                .Returns((Income incomeParameter, CancellationToken cancellationToken) =>
                    Task.FromResult(savedIncome = incomeParameter));
            var viewModel = new IncomesViewModel(repositoryMock.Object);

            await viewModel.AddIncomeCommand.ExecuteAsync(income);

            Assert.AreSame(income, savedIncome);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatingIncomesViewModelWithNullRepositoryThrowsArgumentNullException()
        {
            new IncomesViewModel(null);
        }

        [TestMethod]
        public void InitiallySelectedPageIsEmpty()
        {
            Assert.IsFalse(new IncomesViewModel(new Mock<IncomeRepository>().Object).SelectedPage.Any());
        }
        [TestMethod]
        public async Task TestGetIncomesFromFirstPage()
        {
            var expectedSelectedPage = new Income[0];
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(expectedSelectedPage));

            await viewModel.SelectPageCommand.ExecuteAsync(1);

            Assert.AreSame(expectedSelectedPage, viewModel.SelectedPage);
        }
        [TestMethod]
        public async Task TestGetIncomesFromSecondPage()
        {
            var expectedSelectedPage = new Income[0];

            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(new Income[0], expectedSelectedPage));
            await viewModel.SelectPageCommand.ExecuteAsync(2);

            Assert.AreSame(expectedSelectedPage, viewModel.SelectedPage);
        }
        [TestMethod]
        public async Task TestSelectingPageRaisesPropertyChangedEventAccordingly()
        {
            var raiseCount = 0;
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(new Income[0]));
            viewModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(IncomesViewModel.SelectedPage).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                        raiseCount++;
                };

            await viewModel.SelectPageCommand.ExecuteAsync(1);

            Assert.AreEqual(1, raiseCount);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task TestSelectingPageOutOfBoundsThrowsException()
        {
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages());

            await viewModel.SelectPageCommand.ExecuteAsync(0);
        }
        [TestMethod]
        public async Task TestGetPagesRangeHavingNoPages()
        {
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages());

            await viewModel.LoadPageInfoCommand.ExecuteAsync(null);
            Assert.AreEqual(0, viewModel.PageRange.Count());
        }
        [TestMethod]
        public async Task TestGetPagesRangeHavingTwoPages()
        {
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(new Income[0], new Income[0]));

            await viewModel.LoadPageInfoCommand.ExecuteAsync(null);
            Assert.AreEqual(2, viewModel.PageRange.Count());
        }

        [TestMethod]
        public void TestSelectedPageNumberIsInitiallyNull()
        {
            var viewModel = new IncomesViewModel(new Mock<IIncomeRepository>().Object);

            Assert.IsNull(viewModel.SelectedPageNumber);
        }
        [TestMethod]
        public async Task TestSelectedPageNumberIsEqualToSelectPageParameterAfterCompletion()
        {
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(new Income[0]));

            await viewModel.SelectPageCommand.ExecuteAsync(1);
            Assert.AreEqual(1, viewModel.SelectedPageNumber);
        }
        [TestMethod]
        public async Task TestSelectingPageRaisesPropertyChangedForSelectedPageNumberAccordingly()
        {
            var raiseCount = 0;
            var viewModel = new IncomesViewModel(_GetRepositoryMockWithPages(new Income[0]));
            viewModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (nameof(IncomesViewModel.SelectedPageNumber)
                        .Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                        raiseCount++;
                };

            await viewModel.SelectPageCommand.ExecuteAsync(1);
            Assert.AreEqual(1, raiseCount);
        }

        private static IIncomeRepository _GetRepositoryMockWithPages(params IEnumerable<Income>[] pages)
        {
            var repositoryMock = new Mock<IncomeRepository>();
            repositoryMock
                .Setup(repository => repository.GetOnPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((int pageNumber, CancellationToken cancellationToken) =>
                    {
                        try
                        {
                            return Task.FromResult(pages[pageNumber - 1]);
                        }
                        catch (IndexOutOfRangeException indexOutOfRangeException)
                        {
                            throw new ArgumentOutOfRangeException(nameof(pageNumber), indexOutOfRangeException);
                        }
                    });
            repositoryMock
                .Setup(repository => repository.GetPageCountAsync(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken cancellationToken) => Task.FromResult(pages.Length));

            return repositoryMock.Object;
        }

        [TestMethod]
        public async Task TestFillingIncomesOnTwoPagesRaisesCollectionChangedOnPageRangeAccordingly()
        {
            var pageCount = 0;
            var viewModel = new IncomesViewModel(_GetRepositoryMockForIncomeSaveTests());
            ((INotifyCollectionChanged)viewModel.PageRange).CollectionChanged +=
                (sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add
                        && e.NewItems != null
                        && e.NewItems.Count == 1
                        && (int)e.NewItems[0] == (pageCount + 1))
                        pageCount++;
                };
            await viewModel.LoadPageInfoCommand.ExecuteAsync(null);

            for (var incomeCount = 0; incomeCount < 11; incomeCount++)
                await viewModel.AddIncomeCommand.ExecuteAsync(new Income());

            Assert.AreEqual(2, pageCount);
        }
        [TestMethod]
        public async Task TestPageRangeRaisesCollectionChangedAccordinglyWhenAddingFirstIncome()
        {
            var raiseCount = 0;
            var viewModel = new IncomesViewModel(_GetRepositoryMockForIncomeSaveTests());
            ((INotifyCollectionChanged)viewModel.PageRange).CollectionChanged +=
                (sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add
                        && e.NewItems != null
                        && e.NewItems.Count == 1
                        && (int)e.NewItems[0] == 1)
                        raiseCount++;
                };
            await viewModel.LoadPageInfoCommand.ExecuteAsync(null);
            await viewModel.AddIncomeCommand.ExecuteAsync(new Income());

            Assert.AreEqual(1, raiseCount);
        }

        private static IIncomeRepository _GetRepositoryMockForIncomeSaveTests()
        {
            var incomes = new List<Income>();
            var repositoryMock = new Mock<IncomeRepository>();

            repositoryMock
                .Setup(repository => repository.GetOnPageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((int pageNumber, CancellationToken cancellationToken) =>
                    {
                        if (pageNumber < 1 || incomes.Count / 10 < pageNumber)
                            throw new ArgumentOutOfRangeException(nameof(pageNumber));

                        return Task.FromResult(incomes.Skip((pageNumber - 1) * 10).Take(10));
                    });
            repositoryMock
                .Setup(repository =>
                    repository.GetPageCountAsync(It.IsAny<CancellationToken>()))
                .Returns((CancellationToken cancellationToken) =>
                    Task.FromResult(incomes.Count / 10 + 1));
            repositoryMock
                .Setup(repository => repository.SaveAsync(It.IsAny<Income>(), It.IsAny<CancellationToken>()))
                .Returns((Income income, CancellationToken cancellationToken) =>
                    {
                        incomes.Add(income);
                        return Task.FromResult(default(object));
                    });

            return repositoryMock.Object;
        }
    }
}