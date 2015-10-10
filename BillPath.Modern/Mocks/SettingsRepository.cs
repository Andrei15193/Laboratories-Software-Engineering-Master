using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.Modern.Mocks
{
    public class SettingsRepository
        : DataAccess.SettingsRepository
    {
        private Settings _settings =
            new Settings
            {
                PreferredCurrency = new Currency(new RegionInfo(CultureInfo.CurrentCulture.Name)),
                PreferredCurrencyDisplayFormat = CurrencyDisplayFormat.IsoCode
            };

        public int MillisecondsDelay
        {
            get;
            set;
        }

        public override async Task<Settings> GetAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay, cancellationToken);
            return _settings;
        }

        public override async Task SaveAsync(Settings settings, CancellationToken cancellationToken)
        {
            await Task.Delay(MillisecondsDelay, cancellationToken);
            _settings = settings;
        }
    }
}