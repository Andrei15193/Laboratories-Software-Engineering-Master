using System;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Providers.Tests
{
    [TestClass]
    public class CurrencyProviderTests
    {
        private static CurrencyProvider Provider { get; } = new CurrencyProvider();

        [TestMethod]
        public void TestProviderDoesNotReturnNull()
            => Assert.IsNotNull(Provider.Currencies);

        [TestMethod]
        public void TestProviderReturnsDistinctCurrencies()
            => Assert.AreEqual(
                Provider.Currencies.Count(),
                Provider.Currencies.Distinct().Count(),
                string.Join(
                    Environment.NewLine,
                    new[] { "The following currencies are duplicates:" }.Concat(
                        from currency in Provider.Currencies
                        group currency by currency into currencies
                        where currencies.Skip(1).Any()
                        select string.Join(", ", currencies))));

        [DataTestMethod]
        [DataRow(153)]
        public void TestProviderReturnsAllCurrencies(int currencyCount)
            => Assert.AreEqual(currencyCount, Provider.Currencies.Count());
    }
}