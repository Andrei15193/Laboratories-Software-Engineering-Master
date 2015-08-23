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
        private readonly Settings _settings;

        public SettingsViewModel()
        {
            _settings = new Settings();
            LoadCommand = new DelegateAsyncCommand(_LoadSettings);
            SaveCommand = new DelegateAsyncCommand(_SaveSettings);
        }

        private async Task _LoadSettings(object parameter, CancellationToken cancellationToken)
        {
            var settings = await Repository.GetAsync();

            if (settings != null)
                PreferredCurrency = settings.PreferredCurrency;
            else
                PreferredCurrency = new Currency(new RegionInfo(CultureInfo.CurrentCulture.Name));
        }
        private Task _SaveSettings(object parameter, CancellationToken cancellationToken)
        {
            return Repository.SaveAsync(_settings);
        }

        public ISettingsRepository Repository
        {
            get;
            set;
        }

        public Currency PreferredCurrency
        {
            get
            {
                return _settings.PreferredCurrency;
            }
            set
            {
                _settings.PreferredCurrency = value;
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
        public object Thread { get; private set; }
    }
}