using System;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.DataAccess.Tests
{
    [TestClass]
    public class IncomeRepositoryChangedTests
    {
        [TestMethod]
        public void TestCannotCreateWithNullIncome()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IncomeRepositoryChanged(null));
        }

        [TestMethod]
        public void TestProvidedIncomeIsReturnedByIncomeProperty()
        {
            var expectedIncome = new Income();
            var incomeRepositoryChanged = new IncomeRepositoryChanged(expectedIncome);

            var actualIncome = incomeRepositoryChanged.Income;

            Assert.AreSame(expectedIncome, actualIncome);
        }
    }
}