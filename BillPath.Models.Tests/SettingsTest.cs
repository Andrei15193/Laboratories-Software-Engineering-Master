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
            Assert.AreEqual(Instance.PreferredCurrencyDisplayFormat, other.PreferredCurrencyDisplayFormat);
            Assert.AreEqual(Instance.PreferredCurrency, other.PreferredCurrency);
        }

        protected override void SetValidTestDataToInstance()
        {
            Instance.PreferredCurrencyDisplayFormat = CurrencyDisplayFormat.IsoCode;
            Instance.PreferredCurrency = new Currency(new RegionInfo("en-US"));
        }
    }
}