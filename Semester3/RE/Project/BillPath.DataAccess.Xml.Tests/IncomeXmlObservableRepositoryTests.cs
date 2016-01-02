using System.Threading.Tasks;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Xml.Tests
{
    [TestClass]
    public class IncomeXmlObservableRepositoryTests
    {
        [TestMethod]
        public async Task TestAddingIncomeToRepositoryRaisesCorrespondignEvent()
        {
            var raiseCount = 0;

            using (var repository = new IncomeXmlMockRepository())
            {
                var observableRepository = new IncomeObservableRepository(repository);

                observableRepository.SavedIncome += delegate { raiseCount += 1; };
                await observableRepository.SaveAsync(new Income());
            }

            Assert.AreEqual(1, raiseCount);
        }

        [TestMethod]
        public async Task TestRemovingIncomeFromRepositoryRaisesCorrespondingEvent()
        {
            var raiseCount = 0;
            var income = new Income();

            using (var repository = new IncomeXmlMockRepository())
            {
                await repository.SaveAsync(income);

                var observableRepository = new IncomeObservableRepository(repository);

                observableRepository.RemovedIncome += delegate { raiseCount += 1; };
                await observableRepository.RemoveAsync(income);
            }

            Assert.AreEqual(1, raiseCount);
        }
    }
}