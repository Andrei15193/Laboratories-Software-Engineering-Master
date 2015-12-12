using System;
using System.Globalization;
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
            using (var repository = new IncomeXmlMemoryStreamRepository())
            using (var reader = await repository.GetReaderAsync())
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
            using (var repository = new IncomeXmlMemoryStreamRepository())
            {
                var expectedIncome = new Income();

                for (var count = 0; count < incomeSaveRepeatCount; count++)
                    await repository.SaveAsync(expectedIncome);

                using (var reader = await repository.GetReaderAsync())
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
            using (var repository = new IncomeXmlMemoryStreamRepository())
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

                using (var reader = await repository.GetReaderAsync())
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
            using (var repository = new IncomeXmlMemoryStreamRepository())
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

                using (var reader = await repository.GetReaderAsync())
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
            using (var repository = new IncomeXmlMemoryStreamRepository())
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

                using (var reader = await repository.GetReaderAsync())
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    Assert.IsTrue(await reader.ReadAsync());
                    var actualIncome = reader.Current;

                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                }
            }
        }

        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(2, 0)]
        [DataRow(2, 1)]
        [DataRow(3, 0)]
        [DataRow(3, 1)]
        [DataRow(3, 2)]
        public async Task TestRemovingExistingIncome(int totalIncomeCount, int indexToRemove)
        {
            var incomeToRemove =
                new Income
                {
                    Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                    Description = "Test description " + indexToRemove.ToString()
                };

            using (var repository = new IncomeXmlMemoryStreamRepository())
            {
                for (var incomeIndex = 0; incomeIndex < totalIncomeCount; incomeIndex++)
                    await repository.SaveAsync(
                        new Income
                        {
                            Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                            DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                            Description = "Test description " + incomeIndex.ToString()
                        });

                await repository.RemoveAsync(incomeToRemove);

                using (var reader = await repository.GetReaderAsync())
                    while (await reader.ReadAsync())
                        Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(
                            incomeToRemove,
                            reader.Current));
            }
        }


        [TestMethod]
        public async Task TestRemovingOneIncomeCloneRemovesOnlyOneIncome()
        {
            var income = new Income();
            using (var repository = new IncomeXmlMemoryStreamRepository())
            {
                await repository.SaveAsync(income);
                await repository.SaveAsync(income);

                Assert.AreEqual(2, await _GetIncomeCountAsync(repository));

                await repository.RemoveAsync(income);

                Assert.AreEqual(1, await _GetIncomeCountAsync(repository));
            }
        }

        private static async Task<int> _GetIncomeCountAsync(IncomeXmlRepository repository)
        {
            var incomeCount = 0;

            using (var reader = await repository.GetReaderAsync())
                while (await reader.ReadAsync())
                    incomeCount++;

            return incomeCount;
        }
    }
}