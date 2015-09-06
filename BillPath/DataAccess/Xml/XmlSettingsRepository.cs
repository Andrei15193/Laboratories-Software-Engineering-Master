using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class XmlSettingsRepository
        : SettingsRepository
    {
        private readonly string _fileName;
        private readonly FileProvider _fileProvider;
        private volatile Settings _settings = null;
        
        public XmlSettingsRepository(FileProvider fileProvider, string fileName)
        {
            if (fileProvider == null)
                throw new ArgumentNullException(nameof(fileProvider));
            if (string.IsNullOrWhiteSpace(fileName))
                if (fileName == null)
                    throw new ArgumentNullException(nameof(fileName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(fileName));

            _fileProvider = fileProvider;
            _fileName = fileName;
        }

        public override async Task<Settings> GetAsync(CancellationToken cancellationToken)
        {
            if (_settings != null)
                return _settings;

            if (!(await _fileProvider.FileExistsAsync(_fileName, cancellationToken)))
                return null;
            cancellationToken.ThrowIfCancellationRequested();

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await _fileProvider.GetReadStreamForAsync(_fileName, cancellationToken))
                _settings = (Settings)serializer.ReadObject(settingsStream);
            cancellationToken.ThrowIfCancellationRequested();

            return _settings;
        }
        
        public override async Task SaveAsync(Settings settings, CancellationToken cancellationToken)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var serializer = new DataContractSerializer(typeof(Settings));
            using (var settingsStream = await _fileProvider.GetWriteStreamForAsync(_fileName, cancellationToken))
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