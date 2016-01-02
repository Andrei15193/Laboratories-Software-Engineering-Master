using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class CurrencyIsoCodeOnlyFormatterTests
    {
        [TestMethod]
        public void TestFormatterGetsOnlyCurrencyIsoCodeFromCurrency()
        {
            var formatter = new CurrencyIsoCodeOnlyFormatter();
            var currency = new Currency(new RegionInfo("en-US"));

            Assert.AreEqual(currency.IsoCode, formatter.Format(currency));
        }

        [TestMethod]
        public void TestFormatterGetsEmptyStringForDefaultCurrency()
        {
            var formatter = new CurrencyIsoCodeOnlyFormatter();

            Assert.AreEqual(string.Empty, formatter.Format(default(Currency)));
        }
    }
}