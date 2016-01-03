using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BillPath.Models;
using Windows.Storage;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    public class SettingsRepository
        : DataAccess.SettingsRepository
    {
        private readonly string _fileName;
        private Settings _settings =
            new Settings
            {
                PreferredCurrency = new Currency(new RegionInfo(CultureInfo.CurrentCulture.Name)),
                PreferredCurrencyDisplayFormat = CurrencyDisplayFormat.IsoCode
            };

        public SettingsRepository()
        {
            _fileName = Application.Current.GetResource<string>("SettingsFileName");
        }

        public Settings Settings
        {
            get
            {
                return _settings;
            }

            set
            {
                _settings = value;
            }
        }

        public override async Task<Settings> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var file = await ApplicationData
                    .Current
                    .LocalFolder
                    .CreateFileAsync(
                        _fileName,
                        CreationCollisionOption.OpenIfExists)
                    .AsTask(cancellationToken);
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var serializer = new DataContractSerializer(typeof(Settings));
                    _settings = (Settings)serializer.ReadObject(stream);
                }

                return _settings;
            }
            catch
            {
                return _settings;
            }
        }

        public override async Task SaveAsync(Settings settings, CancellationToken cancellationToken)
        {
            var file = await ApplicationData
                .Current
                .LocalFolder
                .CreateFileAsync(
                    _fileName,
                    CreationCollisionOption.ReplaceExisting)
                .AsTask(cancellationToken);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var serializer = new DataContractSerializer(typeof(Settings));
                serializer.WriteObject(stream, settings);
                _settings = settings;
            }
        }
    }
}