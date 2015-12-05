using System;
using System.ComponentModel;
using BillPath.Modern.Converters;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern.Controls.Display
{
    public sealed partial class AmountView2
        : UserControl
    {
        private static readonly CurrencyDisplayConverter _converter = new CurrencyDisplayConverter();

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register(
                nameof(Amount),
                typeof(ModelState),
                typeof(AmountView2),
                new PropertyMetadata(
                    null,
                    (d, e) => ((AmountView2)d)._UpdateAmountTextBlockText()));

        public AmountView2()
        {
            InitializeComponent();
            Loaded +=
                delegate
                {
                    Application.Current.GetResource<SettingsViewModel>().PropertyChanged += _SettingsViewModelPropertyChanged;
                };
        }

        public ModelState Amount
        {
            get
            {
                return (ModelState)GetValue(AmountProperty);
            }
            set
            {
                SetValue(AmountProperty, value);
            }
        }

        private void _SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(SettingsViewModel.PreferredCurrencyDisplayFormat).Equals(
                e.PropertyName,
                StringComparison.OrdinalIgnoreCase))
                _UpdateAmountTextBlockText();
        }
        private void _UpdateAmountTextBlockText()
        {
            if (Amount != null)
                AmountTextBlock.Text = ((decimal)Amount[nameof(Models.Amount.Value)]).ToString("N") + _GetCurrencyText();
        }

        private string _GetCurrencyText()
            => (string)_converter.Convert(
            Application.Current.GetResource<SettingsViewModel>().PreferredCurrencyDisplayFormat,
            typeof(string),
            _Currency,
            null) ?? string.Empty;
        private Models.Currency _Currency
        {
            get
            {
                var currencyModelState = (ModelState)Amount[nameof(Models.Amount.Currency)];
                var currency = (Models.Currency)currencyModelState.Model;

                return currency;
            }
        }
    }
}