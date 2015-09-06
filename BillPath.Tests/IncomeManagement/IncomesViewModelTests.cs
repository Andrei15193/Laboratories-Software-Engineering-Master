using System;
using System.Globalization;
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

            await viewModel.AddIncome.ExecuteAsync(income);

            Assert.AreSame(income, savedIncome);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreatingIncomesViewModelWithNullRepositoryThrowsArgumentNullException()
        {
            new IncomesViewModel(null);
        }
    }
}