using System;
using System.Globalization;
using System.Threading.Tasks;
using BillPath.DataAccess.Xml.Mock;
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
            using (var repository = new IncomeXmlRepositoryMock())
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

                using (var reader = repository.GetReader())
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
            using (var repository = new IncomeXmlRepositoryMock())
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
    }
}