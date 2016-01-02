using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class CurrencyDisplayFormatTests
    {
        [TestMethod]
        public void TestHasDefaultMember()
        {
            Assert.IsTrue(Enum.IsDefined(typeof(CurrencyDisplayFormat), default(CurrencyDisplayFormat)));
        }

        [TestMethod]
        public void TestFullDisplayFormatIsTheDefault()
        {
            Assert.AreEqual(CurrencyDisplayFormat.Full, default(CurrencyDisplayFormat));
        }
    }
}