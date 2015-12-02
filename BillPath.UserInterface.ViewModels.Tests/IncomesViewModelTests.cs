using System;
using System.Globalization;
using System.Threading.Tasks;
using BillPath.DataAccess.Mocks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class IncomesViewModelTests
    {
        [TestMethod]
        public async Task TestSaveNewValidIncome()
        {
            var repository = new IncomesRepositoryMock();
            var viewModel = new IncomeViewModel(repository);
            var expectedIncome =
                new Income
                {
                    Amount = new Amount(1, new Currency(new RegionInfo("en-AU")))
                };
            viewModel.ModelState = ModelState.GetFor(expectedIncome);

            await viewModel.SaveCommand.ExecuteAsync(null);

            using (var incomeReader = repository.GetReader())
            {
                Assert.IsTrue(await incomeReader.ReadAsync());

                var actualIncome = incomeReader.Current;
                _AssertAreEqual(expectedIncome, actualIncome);

                Assert.IsFalse(await incomeReader.ReadAsync());
            }
        }
        [TestMethod]
        public async Task TestExceptionIsThrownWhenTryingToSaveInvalidIncome()
        {
            var viewModel = new IncomeViewModel(new IncomesRepositoryMock());
            var invalidIncome =
                new Income
                {
                    Amount = new Amount(-1, new Currency(new RegionInfo("en-AU")))
                };
            viewModel.ModelState = ModelState.GetFor(invalidIncome);

            var exception = await viewModel
                .SaveCommand
                .ExecuteAsync(null)
                .ContinueWith(saveTask => saveTask.Exception.InnerException);

            Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
        }
        [TestMethod]
        public async Task TextExceptionIsThrownWhenTryingToSaveNullIncome()
        {
            var viewModel = new IncomeViewModel(new IncomesRepositoryMock());

            var exception = await viewModel
                .SaveCommand
                .ExecuteAsync(null)
                .ContinueWith(saveTask => saveTask.Exception.InnerException);

            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        private void _AssertAreEqual(Income expectedIncome, Income actualIncome)
        {
            Assert.AreEqual(
                expectedIncome.Amount,
                actualIncome.Amount);
            Assert.AreEqual(
                expectedIncome.DateRealized,
                actualIncome.DateRealized);
            Assert.AreEqual(
                expectedIncome.Description,
                actualIncome.Description);
        }

        [TestMethod]
        public void TestExceptionIsThrownWhenCreatingViewModelWithNullRepository()
            => Assert.ThrowsException<ArgumentNullException>(
                () => new IncomesViewModel(null));
    }
}