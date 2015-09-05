using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class SettingsRepository
        : ISettingsRepository
    {
        private volatile Settings _settings = null;


        public FileProvider FileProvider
        {
            get;
            set;
        }

        public Task<Settings> GetAsync()
        {
            return GetAsync(CancellationToken.None);
        }
        public async Task<Settings> GetAsync(CancellationToken cancellationToken)
        {
            if (_settings != null)
                return _settings;

            if (!(await FileProvider.FileExistsAsync(cancellationToken)))
                return null;
            cancellationToken.ThrowIfCancellationRequested();

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await FileProvider.GetReadStreamAsync(cancellationToken))
                _settings = (Settings)serializer.ReadObject(settingsStream);
            cancellationToken.ThrowIfCancellationRequested();

            return _settings;
        }

        public Task SaveAsync(Settings settings)
        {
            return SaveAsync(settings, CancellationToken.None);
        }
        public async Task SaveAsync(Settings settings, CancellationToken cancellationToken)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await FileProvider.GetWriteStreamAsync(cancellationToken))
                serializer.WriteObject(settingsStream, settings);
            cancellationToken.ThrowIfCancellationRequested();

            if (_settings == null)
                _settings = settings;
            else
                _CopyFrom(settings);
        }

        private void _CopyFrom(Settings settings)
        {
            _settings.PreferredCurrency = settings.PreferredCurrency;
            _settings.CurrencyDisplayFormat = settings.CurrencyDisplayFormat;
        }
    }
}