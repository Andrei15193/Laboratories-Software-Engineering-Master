using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class IncomeTests
        : TransactionTests<Income>
    {
        protected override void AssertAreEqual(Income first, Income second)
        {
            Assert.AreEqual(first.Amount, second.Amount);
            Assert.AreEqual(first.DateRealized, second.DateRealized);
            Assert.AreEqual(first.Description, second.Description, ignoreCase: true);
        }
    }
}