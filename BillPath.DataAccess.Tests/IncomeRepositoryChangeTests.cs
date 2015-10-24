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
            => Assert.ThrowsException<ArgumentNullException>(() => new IncomeRepositoryChange(
                null,
                default(IncomeRepositoryChangeAction)));

        [TestMethod]
        public void TestProvidedIncomeIsReturnedByIncomeProperty()
        {
            var expectedIncome = new Income();
            var incomeRepositoryChanged = new IncomeRepositoryChange(
                expectedIncome,
                default(IncomeRepositoryChangeAction));

            var actualIncome = incomeRepositoryChanged.Income;

            Assert.AreSame(
                expectedIncome,
                actualIncome);
        }

        [TestMethod]
        public void TestProvidedActionIsReturnedByActionProperty()
        {
            var expectedAction = IncomeRepositoryChangeAction.Add;
            var incomeRepositoryChange = new IncomeRepositoryChange(
                new Income(),
                expectedAction);

            var actualAction = incomeRepositoryChange.Action;

            Assert.AreEqual(
                expectedAction,
                actualAction);
        }
    }
}