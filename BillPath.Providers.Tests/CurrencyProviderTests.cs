using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Providers.Tests
{
    [TestClass]
    public class CurrencyProviderTests
    {
        [TestMethod]
        public void TestProviderDoesNotReturnNull()
        {
            var currencyProvider = new CurrencyProvider();

            Assert.IsNotNull(currencyProvider.Currencies);
        }

        [TestMethod]
        public void TestProviderReturnsDistinctCurrencies()
        {
            var currencyProvider = new CurrencyProvider();

            Assert.AreEqual(
                currencyProvider.Currencies.Count(),
                currencyProvider.Currencies.Distinct().Count(),
                string.Join(
                    Environment.NewLine,
                    new[] { "The following currencies are duplicates:" }.Concat(
                        from currency in currencyProvider.Currencies
                        group currency by currency into currencies
                        where currencies.Skip(1).Any()
                        select string.Join(", ", currencies))));
        }

        [DataTestMethod]
        [DataRow(153)]
        public void TestProviderReturnsAllCurrencies(int currencyCount)
        {
            var currencyProvider = new CurrencyProvider();

            Assert.AreEqual(currencyCount, currencyProvider.Currencies.Count());
        }
    }
}