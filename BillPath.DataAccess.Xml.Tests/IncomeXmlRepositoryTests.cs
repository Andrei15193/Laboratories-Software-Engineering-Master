using System;
using System.Globalization;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public abstract class IncomeXmlRepositoryTests
    {
        private IIncomeXmlRepository _repository;

        protected abstract IIncomeXmlRepository CreateRepository();

        [TestInitialize]
        public async Task TestInitialize()
        {
            await OnTestInitializingAsync();

            _repository = CreateRepository();

            await OnTestInitializedAsync();
        }
        protected virtual Task OnTestInitializingAsync()
            => Task.FromResult(default(object));
        protected virtual Task OnTestInitializedAsync()
            => Task.FromResult(default(object));

        [TestCleanup]
        public async Task TestCleanup()
        {
            await OnTestCleaningUpAsync();

            (_repository as IDisposable)?.Dispose();
            _repository = null;

            await OnTestCleanedUpAsync();
        }
        protected virtual Task OnTestCleaningUpAsync()
            => Task.FromResult(default(object));
        protected virtual Task OnTestCleanedUpAsync()
            => Task.FromResult(default(object));

        [TestMethod]
        public async Task TestCreatingNewRepositoryRetrievesNoIncomes()
        {
            using (var reader = await _repository.GetReaderAsync())
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
            var expectedIncome = new Income();

            for (var count = 0; count < incomeSaveRepeatCount; count++)
                await _repository.SaveAsync(expectedIncome);

            using (var reader = await _repository.GetReaderAsync())
                for (var count = 0; count < incomeSaveRepeatCount; count++)
                {
                    Assert.IsTrue(await reader.ReadAsync());
                    var actualIncome = reader.Current;

                    Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
                }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeWithLaterDateThanAllIsReturnedFirstByReader()
        {
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 1), new TimeSpan())
                });
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                });

            var expectedIncome =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                };
            await _repository.SaveAsync(expectedIncome);

            using (var reader = await _repository.GetReaderAsync())
            {
                Assert.IsTrue(await reader.ReadAsync());
                var actualIncome = reader.Current;

                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
            }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeWithEarlierDateThanAllIsReturnedLastByReader()
        {
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 4), new TimeSpan())
                });
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 5), new TimeSpan())
                });

            var expectedIncome =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                };
            await _repository.SaveAsync(expectedIncome);

            using (var reader = await _repository.GetReaderAsync())
            {
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(await reader.ReadAsync());
                var actualIncome = reader.Current;

                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
            }
        }

        [TestMethod]
        public async Task TestSavingNewIncomeNotHavingTheRealizedDateBetweenTwoExistingIncomesIsReturnedAsSecondItem()
        {
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 4), new TimeSpan())
                });
            await _repository.SaveAsync(
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 2), new TimeSpan())
                });

            var expectedIncome =
                new Income
                {
                    DateRealized = new DateTimeOffset(new DateTime(2015, 12, 3), new TimeSpan())
                };
            await _repository.SaveAsync(expectedIncome);

            using (var reader = await _repository.GetReaderAsync())
            {
                Assert.IsTrue(await reader.ReadAsync());
                Assert.IsTrue(await reader.ReadAsync());
                var actualIncome = reader.Current;

                Assert.IsTrue(IncomeEqualityComparer.Instance.Equals(expectedIncome, actualIncome));
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

            for (var incomeIndex = 0; incomeIndex < totalIncomeCount; incomeIndex++)
                await _repository.SaveAsync(
                    new Income
                    {
                        Amount = new Amount(100, new Currency(new RegionInfo("en-US"))),
                        DateRealized = new DateTimeOffset(new DateTime(2015, 12, 6), new TimeSpan()),
                        Description = "Test description " + incomeIndex.ToString()
                    });

            await _repository.RemoveAsync(incomeToRemove);

            using (var reader = await _repository.GetReaderAsync())
                while (await reader.ReadAsync())
                    Assert.IsFalse(IncomeEqualityComparer.Instance.Equals(
                        incomeToRemove,
                        reader.Current));
        }

        [TestMethod]
        public async Task TestRemovingOneIncomeCloneRemovesOnlyOneIncome()
        {
            var income = new Income();
            await _repository.SaveAsync(income);
            await _repository.SaveAsync(income);

            Assert.AreEqual(2, await _repository.GetCountAsync());

            await _repository.RemoveAsync(income);

            Assert.AreEqual(1, await _repository.GetCountAsync());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        public async Task TestGetIncomesCount(int incomesCount)
        {
            var income = new Income();

            Assert.AreEqual(0, await _repository.GetCountAsync());

            for (var count = 0; count < incomesCount; count++)
                await _repository.SaveAsync(income);

            Assert.AreEqual(incomesCount, await _repository.GetCountAsync());
        }
    }
}