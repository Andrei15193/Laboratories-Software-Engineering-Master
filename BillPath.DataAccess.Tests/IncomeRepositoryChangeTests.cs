using System;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Tests
{
    [TestClass]
    public class IncomeRepositoryChangeTests
    {
        [TestMethod]
        public void TestCannotCreateWithNullIncome()
            => Assert.ThrowsException<ArgumentNullException>(() => new RepositoryChange<Income>(
                null,
                default(RepositoryChangeAction)));

        [TestMethod]
        public void TestProvidedIncomeIsReturnedByIncomeProperty()
        {
            var expectedIncome = new Income();
            var incomeRepositoryChanged = new RepositoryChange<Income>(
                expectedIncome,
                default(RepositoryChangeAction));

            var actualIncome = incomeRepositoryChanged.Item;

            Assert.AreSame(
                expectedIncome,
                actualIncome);
        }

        [TestMethod]
        public void TestProvidedActionIsReturnedByActionProperty()
        {
            var expectedAction = RepositoryChangeAction.Add;
            var incomeRepositoryChange = new RepositoryChange<Income>(
                new Income(),
                expectedAction);

            var actualAction = incomeRepositoryChange.Action;

            Assert.AreEqual(
                expectedAction,
                actualAction);
        }
    }
}