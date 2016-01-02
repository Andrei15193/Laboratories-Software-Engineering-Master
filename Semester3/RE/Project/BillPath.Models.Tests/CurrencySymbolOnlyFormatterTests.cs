using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class CurrencySymbolOnlyFormatterTests
    {
        [TestMethod]
        public void TestFormatterGetsOnlyCurrencySymbolFromCurrency()
        {
            var formatter = new CurrencySymbolOnlyFormatter();
            var currency = new Currency(new RegionInfo("en-AU"));

            Assert.AreEqual(currency.Symbol, formatter.Format(currency));
        }

        [TestMethod]
        public void TestFormatterGetsEmptyStringForDefaultCurrency()
        {
            var formatter = new CurrencySymbolOnlyFormatter();

            Assert.AreEqual(string.Empty, formatter.Format(default(Currency)));
        }
    }
}