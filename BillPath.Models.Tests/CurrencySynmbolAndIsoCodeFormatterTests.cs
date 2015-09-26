using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class CurrencySynmbolAndIsoCodeFormatterTests
    {
        [TestMethod]
        public void TestFormatterGetsCurrencySymbolFollowedByCurrencyIsoCodeInParenthesisFromCurrency()
        {
            var formatter = new CurrencySynmbolAndIsoCodeFormatter();
            var currency = new Currency(new RegionInfo("en-GB"));

            Assert.AreEqual($"{currency.Symbol}({currency.IsoCode})", formatter.Format(currency));
        }

        [TestMethod]
        public void TestFormatterGetsEmptyStringForDefaultCurrency()
        {
            var formatter = new CurrencySynmbolAndIsoCodeFormatter();

            Assert.AreEqual(string.Empty, formatter.Format(default(Currency)));
        }
    }
}