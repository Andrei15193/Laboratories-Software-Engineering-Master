using System;
using System.Globalization;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Modern.Converters.Tests
{
    [TestClass]
    public class CurrencyDisplayConverterTests
    {
        [TestMethod]
        public void TestConvertCurrencyUsingFullDisplayFormat()
        {
            _AssertFormat("$(USD)", CurrencyDisplayFormat.Full, new Currency(new RegionInfo("en-US")));
        }
        [TestMethod]
        public void TestConvertCurrencyUsingSymbolOnlyDisplayFormat()
        {
            _AssertFormat("£", CurrencyDisplayFormat.Symbol, new Currency(new RegionInfo("en-GB")));
        }
        [TestMethod]
        public void TestConvertCurrencyUsingIsoCodeOnlyDisplayFormat()
        {
            _AssertFormat("AUD", CurrencyDisplayFormat.IsoCode, new Currency(new RegionInfo("en-AU")));
        }

        private void _AssertFormat(string expectedResult, CurrencyDisplayFormat currencyDisplayFormat, Currency currency)
        {
            Assert.AreEqual(
                expectedResult,
                new CurrencyDisplayConverter().Convert(
                    currencyDisplayFormat,
                    typeof(string),
                    currency,
                    null));
        }

        [TestMethod]
        public void TestConvertUsingInvalidCurrencyDisplayFormatThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new CurrencyDisplayConverter().Convert(
                    (CurrencyDisplayFormat)(-1),
                    null,
                    new Currency(new RegionInfo("ro-RO")),
                    null));
        }

        [TestMethod]
        public void TestConvertBackIsNotImplemented()
        {
            Assert.ThrowsException<NotImplementedException>(
                () => new CurrencyDisplayConverter().ConvertBack(null, null, null, null));
        }
    }
}