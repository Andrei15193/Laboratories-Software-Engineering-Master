using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class CurrencyTests
    {
        [TestMethod]
        public void TestCreateCurrency()
        {
            foreach (var regionInfo in from regionName in new[] { "en-US", "en-AU", "en-GB", "ro-RO" }
                                       select new RegionInfo(regionName))
            {
                var currency = new Currency(regionInfo);

                Assert.AreEqual(regionInfo.ISOCurrencySymbol, currency.IsoCode);
                Assert.AreEqual(regionInfo.CurrencySymbol, currency.Symbol);
            }
        }

        [TestMethod]
        public void TestCurrencyIsEqualToItself()
        {
            var currecy = new Currency(new RegionInfo("RO"));

            Assert.AreEqual(currecy, currecy);
        }

        [TestMethod]
        public void TestCurrenciesCreatedWithSameRegionInfoAreEqual()
        {
            var regionInfo = new RegionInfo("RO");
            var currecy1 = new Currency(regionInfo);
            var currecy2 = new Currency(regionInfo);

            Assert.AreEqual(currecy1, currecy2);
            Assert.IsTrue(currecy1.Equals(currecy2));

            Assert.IsTrue(currecy1 == currecy2);
            Assert.IsFalse(currecy1 != currecy2);

            Assert.AreEqual(currecy1.GetHashCode(), currecy2.GetHashCode());
        }

        [TestMethod]
        public void TestCurrenciesCreatedWithDifferentRegionInfosAreNotEqual()
        {
            var currecy1 = new Currency(new RegionInfo("RO"));
            var currecy2 = new Currency(new RegionInfo("GB"));

            Assert.AreNotEqual(currecy1, currecy2);
            Assert.IsFalse(currecy1.Equals(currecy2));

            Assert.IsFalse(currecy1 == currecy2);
            Assert.IsTrue(currecy1 != currecy2);
        }

        [TestMethod]
        public void TestSerializedCurrencyIsEqualAfterDeserialization()
        {
            var currency = new Currency(new RegionInfo("RO"));
            var currencySerializer = new DataContractSerializer(typeof(Currency));

            using (var currencySerializationStream = new MemoryStream())
            {
                currencySerializer.WriteObject(currencySerializationStream, currency);
                currencySerializationStream.Seek(0, SeekOrigin.Begin);

                Assert.AreEqual(currency, (Currency)currencySerializer.ReadObject(currencySerializationStream));
            }
        }
    }
}