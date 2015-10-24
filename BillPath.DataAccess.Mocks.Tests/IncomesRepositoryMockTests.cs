using System;
using System.Linq;
using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Mocks.Tests
{
    [TestClass]
    public class IncomesRepositoryMockTests
    {
        [TestMethod]
        public void TestGetReaderDoesNotReturnNull()
        {
            var incomesRepository = new IncomesRepositoryMock();

            var reader = incomesRepository.GetReader();

            Assert.IsNotNull(reader);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public async Task TestItemCountIsEqualToItemsInProvidedCollection(int expectedIncomesCount)
        {
            var incomesRepository = new IncomesRepositoryMock(Enumerable.Repeat(new Income(), expectedIncomesCount));

            var actualIncomesCount = await incomesRepository.GetItemCountAsync();

            Assert.AreEqual(expectedIncomesCount, actualIncomesCount);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(5)]
        [DataRow(8)]
        [DataRow(13)]
        [DataRow(21)]
        public async Task TestSavingANewIncomeIncrementsTheCountByOne(int incomesCount)
        {
            var incomesRepository = new IncomesRepositoryMock(Enumerable.Repeat(new Income(), incomesCount));

            await incomesRepository.SaveAsync(new Income());
            var actualIncomesCount = await incomesRepository.GetItemCountAsync();

            Assert.AreEqual(incomesCount + 1, actualIncomesCount);
        }

        [TestMethod]
        public async Task TestExpectionIsThrownWhenTryingtoSaveNullIncome()
        {
            var incomesRepository = new IncomesRepositoryMock();

            var exception = await incomesRepository
                .SaveAsync(null)
                .ContinueWith(saveTask => saveTask.Exception.InnerException);

            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public async Task TestNotificationIsSentWhenAnItemIsAddedToTheRepository()
        {
            var incomesRepository = new IncomesRepositoryMock();
            

            await incomesRepository.SaveAsync(new Income());
        }
    }
}