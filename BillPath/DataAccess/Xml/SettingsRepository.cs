using System;
using System.Runtime.Serialization;
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

        public async Task<Settings> GetAsync()
        {
            if (_settings != null)
                return _settings;

            if (!(await FileProvider.FileExistsAsync()))
                return null;

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await FileProvider.GetReadStreamAsync())
                _settings = (Settings)serializer.ReadObject(settingsStream);

            return _settings;
        }

        public async Task SaveAsync(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await FileProvider.GetWriteStreamAsync())
                serializer.WriteObject(settingsStream, settings);

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