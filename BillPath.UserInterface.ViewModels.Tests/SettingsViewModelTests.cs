using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private sealed class SettingsRepositoryMock
            : ISettingsRepository
        {
            public static Currency InitialCurrency { get; } = new Currency(new RegionInfo("ro-RO"));
            public static CurrencyDisplayFormat InitialCurrencyDisplayFormat { get; } = CurrencyDisplayFormat.IsoCode;

            private Settings _settings =
                new Settings
                {
                    PreferredCurrency = InitialCurrency,
                    PreferredCurrencyDisplayFormat = InitialCurrencyDisplayFormat
                };

            public Task<Settings> GetAsync()
                => GetAsync(CancellationToken.None);

            public Task<Settings> GetAsync(CancellationToken cancellationToken)
                => Task.FromResult(_settings);

            public Task SaveAsync(Settings settings)
                => SaveAsync(settings, CancellationToken.None);
            public Task SaveAsync(Settings settings, CancellationToken cancellationToken)
            {
                if (settings == null)
                    throw new ArgumentNullException(nameof(settings));

                _settings = settings;
                return Task.FromResult(default(object));
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _SettingsRepository = new SettingsRepositoryMock();
            _SettingsViewModel = new SettingsViewModel(_SettingsRepository);
        }

        private ISettingsRepository _SettingsRepository
        {
            get;
            set;
        }
        private SettingsViewModel _SettingsViewModel
        {
            get;
            set;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _SettingsViewModel = null;
            _SettingsRepository = null;
        }

        [TestMethod]
        public void TestAccessingPreferredCurrencyWithoutAwaingLoadThrowsException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _SettingsViewModel.PreferredCurrency);
        }
        [TestMethod]
        public void TestAccessingPreferredCurrencyDisplayFormatWithoutAwaitngLoadThrowsException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _SettingsViewModel.PreferredCurrencyDisplayFormat);
        }

        [TestMethod]
        public async Task TestPreferredCurrencyIsSetAfterLoad()
        {
            await _SettingsViewModel.LoadCommand.ExecuteAsync(null);

            Assert.AreEqual(
                SettingsRepositoryMock.InitialCurrency,
                _SettingsViewModel.PreferredCurrency);
        }
        [TestMethod]
        public async Task TestPreferredCurrencyDisplayFormatIsSetAfterLoad()
        {
            await _SettingsViewModel.LoadCommand.ExecuteAsync(null);

            Assert.AreEqual(
                SettingsRepositoryMock.InitialCurrencyDisplayFormat,
                _SettingsViewModel.PreferredCurrencyDisplayFormat);
        }

        [TestMethod]
        public void TestCannotSaveWithoutLoad()
        {
            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    try
                    {
                        _SettingsViewModel.SaveCommand.ExecuteAsync(null).Wait();
                    }
                    catch (AggregateException aggregateException)
                    {
                        throw aggregateException.InnerException;
                    }
                });
        }
        [TestMethod]
        public async Task TestSettingPreferredCurrencyIsStoredAfterSave()
        {
            var newPreferredCurrency = new Currency(new RegionInfo("en-AU"));
            await _SettingsViewModel.LoadCommand.ExecuteAsync(null);

            _SettingsViewModel.PreferredCurrency = newPreferredCurrency;

            await _SettingsViewModel.SaveCommand.ExecuteAsync(null);

            Assert.AreEqual(
                newPreferredCurrency,
                (await _SettingsRepository.GetAsync()).PreferredCurrency);
        }
        [TestMethod]
        public async Task TestSettingPreferredCurrencyDisplayFormatIsStoredAfterSave()
        {
            var newPreferredCurrencyDisplayFormat = CurrencyDisplayFormat.Symbol;
            await _SettingsViewModel.LoadCommand.ExecuteAsync(null);

            _SettingsViewModel.PreferredCurrencyDisplayFormat = newPreferredCurrencyDisplayFormat;

            await _SettingsViewModel.SaveCommand.ExecuteAsync(null);

            Assert.AreEqual(
                newPreferredCurrencyDisplayFormat,
                (await _SettingsRepository.GetAsync()).PreferredCurrencyDisplayFormat);
        }
    }
}