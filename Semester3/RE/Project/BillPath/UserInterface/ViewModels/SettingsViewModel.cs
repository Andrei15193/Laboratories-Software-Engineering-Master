using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class SettingsViewModel
        : ViewModel
    {
        private bool _loaded;
        private readonly ISettingsRepository _repository;
        private readonly Settings _settings;

        public SettingsViewModel(ISettingsRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _loaded = false;
            _repository = repository;
            _settings = new Settings();
            LoadCommand = new DelegateAsyncCommand(_LoadSettings);
            SaveCommand = new DelegateAsyncCommand(_SaveSettings);
        }

        private async Task _LoadSettings(object parameter, CancellationToken cancellationToken)
        {
            var settings = await _repository.GetAsync(cancellationToken);

            if (settings == null)
                _settings.PreferredCurrency = new Currency(new RegionInfo(CultureInfo.CurrentCulture.Name));
            else
            {
                _settings.PreferredCurrency = settings.PreferredCurrency;
                _settings.PreferredCurrencyDisplayFormat = settings.PreferredCurrencyDisplayFormat;
            }

            _loaded = true;
        }
        private Task _SaveSettings(object parameter, CancellationToken cancellationToken)
        {
            _EnsureIsLoaded();

            return _repository.SaveAsync(_settings, cancellationToken);
        }

        private void _EnsureIsLoaded()
        {
            if (!_loaded)
                throw new InvalidOperationException("Execute " + nameof(LoadCommand) + " command before accessing any properties");
        }

        public Currency PreferredCurrency
        {
            get
            {
                _EnsureIsLoaded();

                return _settings.PreferredCurrency;
            }
            set
            {
                _EnsureIsLoaded();

                _settings.PreferredCurrency = value;
                OnPropertyChanged();
            }
        }

        public CurrencyDisplayFormat PreferredCurrencyDisplayFormat
        {
            get
            {
                _EnsureIsLoaded();

                return _settings.PreferredCurrencyDisplayFormat;
            }
            set
            {
                _EnsureIsLoaded();

                _settings.PreferredCurrencyDisplayFormat = value;
                OnPropertyChanged();
            }
        }

        public AsyncCommand LoadCommand
        {
            get;
        }
        public AsyncCommand SaveCommand
        {
            get;
        }
    }
}