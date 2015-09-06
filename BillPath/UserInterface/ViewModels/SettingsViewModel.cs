using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillPath.DataAccess;
using BillPath.Models;

namespace BillPath.UserInterface.ViewModels
{
    public class SettingsViewModel
        : ViewModel
    {
        private static readonly IEnumerable<Currency> _allCurrencies =
            new[]
            {
                new Currency(new RegionInfo("ar-AE")),
                new Currency(new RegionInfo("prs-AF")),
                new Currency(new RegionInfo("uz-Arab-AF")),
                new Currency(new RegionInfo("sq-AL")),
                new Currency(new RegionInfo("hy-AM")),
                new Currency(new RegionInfo("es-AR")),
                new Currency(new RegionInfo("en-AU")),
                new Currency(new RegionInfo("az-Cyrl-AZ")),
                new Currency(new RegionInfo("bs-Cyrl-BA")),
                new Currency(new RegionInfo("bn-BD")),
                new Currency(new RegionInfo("bg-BG")),
                new Currency(new RegionInfo("ar-BH")),
                new Currency(new RegionInfo("ms-BN")),
                new Currency(new RegionInfo("es-BO")),
                new Currency(new RegionInfo("pt-BR")),
                new Currency(new RegionInfo("dz-BT")),
                new Currency(new RegionInfo("tn-BW")),
                new Currency(new RegionInfo("be-BY")),
                new Currency(new RegionInfo("en-BZ")),
                new Currency(new RegionInfo("en-CA")),
                new Currency(new RegionInfo("fr-CD")),
                new Currency(new RegionInfo("de-CH")),
                new Currency(new RegionInfo("arn-CL")),
                new Currency(new RegionInfo("bo-CN")),
                new Currency(new RegionInfo("es-CO")),
                new Currency(new RegionInfo("es-CR")),
                new Currency(new RegionInfo("es-CU")),
                new Currency(new RegionInfo("cs-CZ")),
                new Currency(new RegionInfo("da-DK")),
                new Currency(new RegionInfo("es-DO")),
                new Currency(new RegionInfo("ar-DZ")),
                new Currency(new RegionInfo("ar-EG")),
                new Currency(new RegionInfo("ti-ER")),
                new Currency(new RegionInfo("am-ET")),
                new Currency(new RegionInfo("br-FR")),
                new Currency(new RegionInfo("cy-GB")),
                new Currency(new RegionInfo("ka-GE")),
                new Currency(new RegionInfo("es-GT")),
                new Currency(new RegionInfo("en-HK")),
                new Currency(new RegionInfo("es-HN")),
                new Currency(new RegionInfo("hr-HR")),
                new Currency(new RegionInfo("fr-HT")),
                new Currency(new RegionInfo("hu-HU")),
                new Currency(new RegionInfo("en-ID")),
                new Currency(new RegionInfo("he-IL")),
                new Currency(new RegionInfo("as-IN")),
                new Currency(new RegionInfo("ar-IQ")),
                new Currency(new RegionInfo("fa-IR")),
                new Currency(new RegionInfo("is-IS")),
                new Currency(new RegionInfo("en-JM")),
                new Currency(new RegionInfo("ar-JO")),
                new Currency(new RegionInfo("ja-JP")),
                new Currency(new RegionInfo("sw-KE")),
                new Currency(new RegionInfo("ky-KG")),
                new Currency(new RegionInfo("km-KH")),
                new Currency(new RegionInfo("ko-KR")),
                new Currency(new RegionInfo("ar-KW")),
                new Currency(new RegionInfo("kk-KZ")),
                new Currency(new RegionInfo("lo-LA")),
                new Currency(new RegionInfo("ar-LB")),
                new Currency(new RegionInfo("si-LK")),
                new Currency(new RegionInfo("ar-LY")),
                new Currency(new RegionInfo("ar-MA")),
                new Currency(new RegionInfo("ro-MD")),
                new Currency(new RegionInfo("mk-MK")),
                new Currency(new RegionInfo("my-MM")),
                new Currency(new RegionInfo("mn-MN")),
                new Currency(new RegionInfo("zh-MO")),
                new Currency(new RegionInfo("dv-MV")),
                new Currency(new RegionInfo("es-MX")),
                new Currency(new RegionInfo("en-MY")),
                new Currency(new RegionInfo("bin-NG")),
                new Currency(new RegionInfo("es-NI")),
                new Currency(new RegionInfo("nb-NO")),
                new Currency(new RegionInfo("ne-NP")),
                new Currency(new RegionInfo("en-NZ")),
                new Currency(new RegionInfo("ar-OM")),
                new Currency(new RegionInfo("es-PA")),
                new Currency(new RegionInfo("es-PE")),
                new Currency(new RegionInfo("en-PH")),
                new Currency(new RegionInfo("pa-Arab-PK")),
                new Currency(new RegionInfo("pl-PL")),
                new Currency(new RegionInfo("es-PY")),
                new Currency(new RegionInfo("ar-QA")),
                new Currency(new RegionInfo("ro-RO")),
                new Currency(new RegionInfo("sr-Cyrl-RS")),
                new Currency(new RegionInfo("ba-RU")),
                new Currency(new RegionInfo("rw-RW")),
                new Currency(new RegionInfo("ar-SA")),
                new Currency(new RegionInfo("se-SE")),
                new Currency(new RegionInfo("en-SG")),
                new Currency(new RegionInfo("so-SO")),
                new Currency(new RegionInfo("ar-SY")),
                new Currency(new RegionInfo("th-TH")),
                new Currency(new RegionInfo("tg-Cyrl-TJ")),
                new Currency(new RegionInfo("tk-TM")),
                new Currency(new RegionInfo("ar-TN")),
                new Currency(new RegionInfo("tr-TR")),
                new Currency(new RegionInfo("en-TT")),
                new Currency(new RegionInfo("zh-TW")),
                new Currency(new RegionInfo("uk-UA")),
                new Currency(new RegionInfo("chr-Cher-US")),
                new Currency(new RegionInfo("es-UY")),
                new Currency(new RegionInfo("uz-Cyrl-UZ")),
                new Currency(new RegionInfo("es-VE")),
                new Currency(new RegionInfo("vi-VN")),
                new Currency(new RegionInfo("fr-CM")),
                new Currency(new RegionInfo("en-029")),
                new Currency(new RegionInfo("es-419")),
                new Currency(new RegionInfo("ff-Latn-SN")),
                new Currency(new RegionInfo("ar-YE")),
                new Currency(new RegionInfo("af-ZA"))
            };
        private static IEnumerable<CurrencyDisplayFormat> _allCurrencyDIsplayFormats =
            Enum.GetValues(typeof(CurrencyDisplayFormat))
                .Cast<CurrencyDisplayFormat>()
                .ToList();
        private readonly ISettingsRepository _repository;
        private readonly Settings _settings;

        public SettingsViewModel(ISettingsRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
            _settings = new Settings();
            LoadCommand = new DelegateAsyncCommand(_LoadSettings);
            SaveCommand = new DelegateAsyncCommand(_SaveSettings);
        }

        private async Task _LoadSettings(object parameter, CancellationToken cancellationToken)
        {
            var settings = await _repository.GetAsync();

            if (settings == null)
                PreferredCurrency = new Currency(new RegionInfo(CultureInfo.CurrentCulture.Name));
            else
            {
                PreferredCurrency = settings.PreferredCurrency;
                CurrencyDisplayFormat = settings.CurrencyDisplayFormat;
            }
        }
        private Task _SaveSettings(object parameter, CancellationToken cancellationToken)
        {
            return _repository.SaveAsync(_settings);
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
        public IEnumerable<Currency> AllCurrencies
        {
            get
            {
                return _allCurrencies;
            }
        }

        public CurrencyDisplayFormat CurrencyDisplayFormat
        {
            get
            {
                return _settings.CurrencyDisplayFormat;
            }
            set
            {
                _settings.CurrencyDisplayFormat = value;
                OnPropertyChanged();
            }
        }
        public IEnumerable<CurrencyDisplayFormat> AllCurrencyDisplayFormats
        {
            get
            {
                return _allCurrencyDIsplayFormats;
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