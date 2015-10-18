using System;
using System.ComponentModel;
using BillPath.Models;
using BillPath.Modern.Converters;
using BillPath.Modern.ResourceBinders;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public sealed partial class AmountView
        : UserControl
    {
        private static readonly CurrencyDisplayConverter _converter = new CurrencyDisplayConverter();

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register(
                nameof(Amount),
                typeof(Amount),
                typeof(AmountView),
                new PropertyMetadata(
                    new Amount(),
                    (d, e) => ((AmountView)d)._UpdateAmountTextBlockText()));

        public AmountView()
        {
            InitializeComponent();
            Loaded +=
                delegate
                {
                    var settingsViewModel = (SettingsViewModel)Application.Current.Resources[nameof(SettingsViewModel)];
                    settingsViewModel.PropertyChanged += _SettingsViewModelPropertyChanged;
                };
        }

        public Amount Amount
        {
            get
            {
                return (Amount)GetValue(AmountProperty);
            }
            set
            {
                SetValue(AmountProperty, value);
            }
        }

        private void _SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(SettingsViewModel.PreferredCurrencyDisplayFormat).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                _UpdateAmountTextBlockText();
        }
        private void _UpdateAmountTextBlockText()
        {
            AmountTextBlock.Text = Amount.Value.ToString("g") + _GetCurrencyText();
        }

        private string _GetCurrencyText()
        {
            return (string)_converter.Convert(
                ((SettingsViewModel)Application.Current.Resources[nameof(SettingsViewModel)]).PreferredCurrencyDisplayFormat,
                typeof(string),
                Amount.Currency,
                null) ?? string.Empty;
        }
    }
}