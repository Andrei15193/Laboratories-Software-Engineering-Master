using System;
using System.Globalization;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class IncomeViewModelTests
    {
        [TestMethod]
        public async Task TestInvokingSaveCommandSavesTheModelFromContextToRepository()
        {
            using (var repository = new IncomeXmlMockRepository())
            {
                var viewModel = new IncomeViewModel(repository);
                var expectedIncome =
                    new Income
                    {
                        Amount = new Amount(100, new Currency(new RegionInfo("en-AU"))),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 4)),
                        Description = "Test description"
                    };

                viewModel.ModelState = new ModelState(expectedIncome.Clone());
                await viewModel.SaveCommand.ExecuteAsync(null);

                using (var reader = await repository.GetReaderAsync())
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(
                        expectedIncome,
                        reader.Current));
                }
            }
        }

        [TestMethod]
        public void TestTryingToCreateAViewModelWithNullRepositoryThrowsException()
            => Assert.ThrowsException<ArgumentNullException>(() => new IncomeViewModel(null));

        [TestMethod]
        public void TestMakingTheModelStateValidEnablesTheSaveCommand()
        {
            using (var repository = new IncomeXmlMockRepository())
            {
                var viewModel =
                    new IncomeViewModel(repository)
                    {
                        ModelState = new ModelState(
                            new Income
                            {
                                Amount = new Amount(0, new Currency(new RegionInfo("en-US")))
                            })
                    };
                Assert.IsFalse(viewModel.SaveCommand.CanExecute);

                viewModel.ModelState[nameof(Income.Amount)] = new Amount(100, new Currency(new RegionInfo("en-US")));
                Assert.IsTrue(viewModel.SaveCommand.CanExecute);
            }
        }

        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        public async Task TestExecutingRemoveIncomeCommandRemovesIncomeFromRepository(int totalIncomeCount, int indexToRemove)
        {
            var incomeToRemove =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                    Description = "Test description " + indexToRemove.ToString()
                };

            using (var repository = new IncomeXmlMockRepository())
            {
                for (var incomeIndex = 0; incomeIndex < totalIncomeCount; incomeIndex++)
                    await repository.SaveAsync(
                        new Income
                        {
                            Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                            DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                            Description = "Test description " + incomeIndex.ToString()
                        });

                var viewModel = new IncomeViewModel(repository, incomeToRemove);
                await viewModel.RemoveCommand.ExecuteAsync(null);

                using (var reader = await repository.GetReaderAsync())
                    while (await reader.ReadAsync())
                        Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(
                            incomeToRemove,
                            reader.Current));
            }
        }
    }
}