using System;
using System.ComponentModel;
using BillPath.Models;
using BillPath.Modern.Converters;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public sealed partial class CurrencyTextBlock
        : UserControl
    {
        private static readonly CurrencyDisplayConverter _converter = new CurrencyDisplayConverter();

        public static readonly DependencyProperty CurrencyProperty =
            DependencyProperty.Register(
                nameof(Currency),
                typeof(Currency),
                typeof(CurrencyTextBlock),
                new PropertyMetadata(
                    new Currency(),
                    (d, e) => ((CurrencyTextBlock)d)._SetCurrencyText()));

        public CurrencyTextBlock()
        {
            InitializeComponent();

            Loaded += delegate
            {
                var settingsViewModel = (SettingsViewModel)Application.Current.Resources[nameof(SettingsViewModel)];
                settingsViewModel.PropertyChanged += _SettingsViewModelPropertyChanged;
            };
        }

        public Currency Currency
        {
            get
            {
                return (Currency)GetValue(CurrencyProperty);
            }
            set
            {
                SetValue(CurrencyProperty, value);
            }
        }
        private TextBlock TextBlock
        {
            get
            {
                return (TextBlock)Content;
            }
            set
            {
                Content = value;
            }
        }

        private void _SettingsViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(SettingsViewModel.PreferredCurrencyDisplayFormat).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                _SetCurrencyText();
        }
        private void _SetCurrencyText()
        {
            var textBlock = TextBlock;
            if (textBlock != null)
                textBlock.Text = (string)_converter.Convert(
                    ((SettingsViewModel)Application.Current.Resources[nameof(SettingsViewModel)]).PreferredCurrencyDisplayFormat,
                    typeof(string),
                    Currency,
                    null) ?? string.Empty;
        }
    }
}