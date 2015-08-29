using System.Globalization;
using BillPath.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BillPath.Tests.CurrencyManagement
{
    [Binding]
    [Scope(Feature = "SetCurrencyDisplayFormat")]
    public sealed class SetCurrencyDisplayFormat
    {
        private Currency _currency;
        private ICurrencyFormatter _currencyFormatter;

        [Given(@"(\w+(?:-\w+)?) currency")]
        public void GivenCurrency(string regionName)
        {
            _currency = new Currency(new RegionInfo(regionName));
        }

        [When("I display currency with symbol only")]
        public void WhenSymbolOnlyDisplayIsSet()
        {
            _currencyFormatter = new CurrencySymbolOnlyFormatter();
        }
        [When("I display currency with ISO code only")]
        public void WhenIsoCodeOnlyDisplayIsSet()
        {
            _currencyFormatter = new CurrencyIsoCodeOnlyFormatter();
        }
        [When("I display currency with symbol and ISO code")]
        public void WhenFullDisplayIsSet()
        {
            _currencyFormatter = new CurrencySynmbolAndIsoCodeFormatter();
        }

        [Then(@"the result should be (.*)")]
        public void ThenFormattedCurrencyIs(string value)
        {
            Assert.AreEqual(value, _currencyFormatter.Format(_currency), ignoreCase: false);
        }
    }
}