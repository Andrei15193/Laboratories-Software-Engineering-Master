using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;

namespace BillPath.Tests.CurrencyManagement
{
    [Binding]
    [Scope(Feature = "SetPreferredCurrency")]
    public sealed class SetPreferredCurrency
    {
        private readonly SettingsViewModel _settingsViewModel;
        private CultureInfo _cultureInfo;
        private Currency? _mockRepositoryCurrency;

        public SetPreferredCurrency()
        {
            _settingsViewModel =
                new SettingsViewModel
                {
                    _repository = _GetScenarioRepository()
                };
            _cultureInfo = null;
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
                        if (settings == null)
                            _mockRepositoryCurrency = null;
                        else
                            _mockRepositoryCurrency = settings.PreferredCurrency;

                        return Task.FromResult(default(object));
                    });
            settingsRepositoryMock
                .Setup(settingsRepository => settingsRepository.GetAsync())
                .Returns(
                    () =>
                    {
                        if (_mockRepositoryCurrency == null)
                            return Task.FromResult<Settings>(null);
                        else
                            return Task.FromResult(
                                new Settings
                                {
                                    PreferredCurrency = _mockRepositoryCurrency.Value
                                });
                    });

            return settingsRepositoryMock.Object;
        }

        [Given(@"the (\w+(?:-\w+)?) currency")]
        public void GivenCurrency(string regionName)
        {
            _settingsViewModel.PreferredCurrency = new Currency(new RegionInfo(regionName));
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
            await _settingsViewModel.SaveCommand.ExecuteAsync(null);
        }
        [When(@"I change the preferred currency to (\w+(?:-\w+)?)")]
        public async Task WhenIChangeThePrefferedCurrency(string regionName)
        {
            _settingsViewModel.PreferredCurrency = new Currency(new RegionInfo(regionName));
            await _settingsViewModel.SaveCommand.ExecuteAsync(null);
        }
        [When("I retrieve the preferred currency")]
        public async Task WhenIGetThePreferredCurrency()
        {
            if (_cultureInfo != null)
                Thread.CurrentThread.CurrentCulture = _cultureInfo;

            await _settingsViewModel.LoadCommand.ExecuteAsync(null);
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
            Assert.AreEqual(new Currency(new RegionInfo(regionName)), _settingsViewModel.PreferredCurrency);
        }
    }
}