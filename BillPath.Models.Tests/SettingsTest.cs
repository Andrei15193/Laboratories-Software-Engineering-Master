using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class SettingsTest
        : CloningTests<Settings>
    {
        protected override void AssertInstanceIsEqualTo(Settings other)
        {
            Assert.AreEqual(Instance.CurrencyDisplayFormat, other.CurrencyDisplayFormat);
            Assert.AreEqual(Instance.PreferredCurrency, other.PreferredCurrency);
        }

        protected override void SetValidTestDataToInstance()
        {
            Instance.CurrencyDisplayFormat = CurrencyDisplayFormat.IsoCode;
            Instance.PreferredCurrency = new Currency(new RegionInfo("en-US"));
        }
    }
}