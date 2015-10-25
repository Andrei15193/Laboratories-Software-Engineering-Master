using System;
using System.ComponentModel;
using BillPath.Modern.Converters;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern.Controls.Display
{
    public sealed partial class AmountView
        : UserControl
    {
        private static readonly CurrencyDisplayConverter _converter = new CurrencyDisplayConverter();

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register(
                nameof(Amount),
                typeof(AmountViewModel),
                typeof(AmountView),
                new PropertyMetadata(
                    null,
                    (d, e) => ((AmountView)d)._UpdateAmountTextBlockText()));

        public AmountView()
        {
            InitializeComponent();
            Loaded +=
                delegate
                {
                    Application.Current.GetResource<SettingsViewModel>().PropertyChanged += _SettingsViewModelPropertyChanged;
                };
        }

        public AmountViewModel Amount
        {
            get
            {
                return (AmountViewModel)GetValue(AmountProperty);
            }
            set
            {
                SetValue(AmountProperty, value);
            }
        }

        private void _SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(ResourceBinders.SettingsViewModel.PreferredCurrencyDisplayFormat).Equals(
                e.PropertyName,
                StringComparison.OrdinalIgnoreCase))
                _UpdateAmountTextBlockText();
        }
        private void _UpdateAmountTextBlockText()
        {
            AmountTextBlock.Text = Amount.Value.ToString("N") + _GetCurrencyText();
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