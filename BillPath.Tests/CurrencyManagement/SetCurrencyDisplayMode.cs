using System.Globalization;
using BillPath.Models;
using TechTalk.SpecFlow;

namespace BillPath.Tests.CurrencyManagement
{
    [Binding]
    [Scope(Feature = "SetCurrencyDisplayMode")]
    public sealed class SetCurrencyDisplayMode
    {
        private Currency _currency;

        [Given(@"(\w+(?:-\w+)?) currency")]
        public void GivenCurrency(string regionName)
        {
            _currency = new Currency(new RegionInfo(regionName));
        }

        [When("I display currency with symbol only")]
        public void WhenSymbolOnlyDisplayIsSet()
        {
            ScenarioContext.Current.Pending();
        }
        [When("I display currency with ISO code only")]
        public void WhenIsoCodeOnlyDisplayIsSet()
        {
            ScenarioContext.Current.Pending();
        }
        [When("I display currency with symbol and ISO code")]
        public void WhenFullDisplayIsSet()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be (.*)")]
        public void ThenFormattedCurrencyIs(string value)
        {
            ScenarioContext.Current.Pending();
        }
    }
}