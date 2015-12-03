using System;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Mock.Tests
{
    [TestClass]
    public class IncomeXmlRepositoryMockTests
    {
        [TestMethod]
        public async Task TestCreatingNewRepositoryRetrievesNoIncomes()
        {
            using (var repository = new IncomeXmlRepositoryMock())
            using (var reader = repository.GetReader())
                Assert.IsFalse(await reader.ReadAsync());
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        public async Task TestSavingAnIncomeRetrievesItWithReader(int incomeSaveRepeatCount)
        {
            using (var repository = new IncomeXmlRepositoryMock())
            {
                var expectedIncome = new Income();

                for (var count = 0; count < incomeSaveRepeatCount; count++)
                    await repository.SaveAsync(expectedIncome);

                using (var reader = repository.GetReader())
                    for (var count = 0; count < incomeSaveRepeatCount; count++)
                    {
                        Assert.IsTrue(await reader.ReadAsync());
                        var actualIncome = reader.Current;

                        Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                    }
            }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeWithLaterDateThanAllIsReturnedFirstByReader()
        {
            using (var repository = new IncomeXmlRepositoryMock())
            {
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan())
                    });
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                    });

                var expectedIncome =
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                    };
                await repository.SaveAsync(expectedIncome);

                using (var reader = repository.GetReader())
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    var actualIncome = reader.Current;

                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                }
            }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeWithEarlierDateThanAllIsReturnedLastByReader()
        {
            using (var repository = new IncomeXmlRepositoryMock())
            {
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 4), new TimeSpan())
                    });
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5), new TimeSpan())
                    });

                var expectedIncome =
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                    };
                await repository.SaveAsync(expectedIncome);

                using (var reader = repository.GetReader())
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    Assert.IsTrue(await reader.ReadAsync());
                    Assert.IsTrue(await reader.ReadAsync());
                    var actualIncome = reader.Current;

                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                }
            }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeNotHavingTheRealizedDateBetweenTwoExistingIncomesIsReturnedAsSecondItem()
        {
            using (var repository = new IncomeXmlRepositoryMock())
            {
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 4), new TimeSpan())
                    });
                await repository.SaveAsync(
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                    });

                var expectedIncome =
                    new Income
                    {
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                    };
                await repository.SaveAsync(expectedIncome);

                using (var reader = repository.GetReader())
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    Assert.IsTrue(await reader.ReadAsync());
                    var actualIncome = reader.Current;

                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                }
            }
        }
    }
}