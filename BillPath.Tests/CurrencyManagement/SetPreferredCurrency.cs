using System.Globalization;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;

namespace BillPath.Tests.CurrencyManagement
{
    [Binding]
    [Scope(Feature = "SetPreferredCurrency")]
    public sealed class SetPreferredCurrency
    {
        private ISettingsRepository _settingsRepository;
        private CultureInfo _cultureInfo;
        private Currency? _scenarioCurrency;
        private Currency? _mockRepositoryCurrency;

        public SetPreferredCurrency()
        {
            _settingsRepository = _GetScenarioRepository();
            _cultureInfo = CultureInfo.CurrentCulture;
            _scenarioCurrency = null;
            _mockRepositoryCurrency = null;
        }
        private ISettingsRepository _GetScenarioRepository()
        {
            var settingsRepositoryMock = new Mock<ISettingsRepository>();

            settingsRepositoryMock
                .Setup(settingsRepository => settingsRepository.SaveAsync(It.IsAny<Settings>()))
                .Returns(
                    (Settings settings) =>
                    {
                        _mockRepositoryCurrency = settings?.PreferredCurrency;

                        return Task.FromResult(default(object));
                    });
            settingsRepositoryMock
                .Setup(settingsRepository => settingsRepository.GetAsync())
                .Returns(
                    () => Task.FromResult(
                        new Settings
                        {
                            PreferredCurrency = _mockRepositoryCurrency ?? new Currency(new RegionInfo(_cultureInfo.Name))
                        }));

            return settingsRepositoryMock.Object;
        }

        [Given(@"the (\w+(?:-\w+)?) currency")]
        public void GivenCurrency(string regionName)
        {
            _scenarioCurrency = new Currency(new RegionInfo(regionName));
        }
        [Given(@"the stored (\w+(?:-\w+)?) currency")]
        public void GivenStoredCurrency(string regionName)
        {
            _mockRepositoryCurrency = new Currency(new RegionInfo(regionName));
        }
        [Given("no stored currency")]
        public void GivenNoStoredCurrency()
        {
            _mockRepositoryCurrency = null;
        }
        [Given(@"(\w+(?:-\w+)?) as current culture")]
        public void SetCurrentCulture(string cultureName)
        {
            _cultureInfo = new CultureInfo(cultureName);
        }

        [When("I set the preferred currency")]
        public async Task WhenISaveThePrefferedCurrency()
        {
            await _settingsRepository.SaveAsync(new Settings { PreferredCurrency = _scenarioCurrency.Value });
        }
        [When(@"I change the preferred currency to (\w+(?:-\w+)?)")]
        public async Task WhenIChangeThePrefferedCurrency(string regionName)
        {
            await _settingsRepository.SaveAsync(
                new Settings
                {
                    PreferredCurrency = new Currency(new RegionInfo(regionName))
                });
        }
        [When("I retrieve the preferred currency")]
        public async Task WhenIGetThePreferredCurrency()
        {
            var settings = await _settingsRepository.GetAsync();
            _scenarioCurrency = settings.PreferredCurrency;
        }

        [Then(@"the (\w+(?:-\w+)?) currency should be stored")]
        public void ThenTheSaveCurrencyIs(string regionName)
        {
            Assert.AreEqual(new Currency(new RegionInfo(regionName)), _mockRepositoryCurrency);
        }
        [Then(@"the (\w+(?:-\w+)?) currency should not be stored")]
        public void ThenTheSaveCurrencyIsNot(string regionName)
        {
            Assert.AreNotEqual(new Currency(new RegionInfo(regionName)), _mockRepositoryCurrency);
        }
        [Then(@"it should be the (\w+(?:-\w+)?) currency")]
        public void ThenRetrievedCurrencyShouldBe(string regionName)
        {
            Assert.AreEqual(new Currency(new RegionInfo(regionName)), _scenarioCurrency);
        }
    }
}