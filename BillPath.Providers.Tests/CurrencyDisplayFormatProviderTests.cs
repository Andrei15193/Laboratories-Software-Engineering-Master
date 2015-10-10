using System;
using System.Linq;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Providers.Tests
{
    [TestClass]
    public class CurrencyDisplayFormatProviderTests
    {
        private static CurrencyDisplayFormatProvider Provider { get; } = new CurrencyDisplayFormatProvider();

        [TestMethod]
        public void TestProviderDoesNotReturnNull()
            => Assert.IsNotNull(Provider.CurrencyDisplayFormats);

        [TestMethod]
        public void TestProviderReturnsDistinctValues()
            => Assert.AreEqual(
                Provider.CurrencyDisplayFormats.Count(),
                Provider.CurrencyDisplayFormats.Distinct().Count());

        [TestMethod]
        public void TestProviderReturnsAnItemForEachDisplayFormat()
            => Assert.AreEqual(
                Enum.GetValues(typeof(CurrencyDisplayFormat)).Length,
                Provider.CurrencyDisplayFormats.Count());
    }
}