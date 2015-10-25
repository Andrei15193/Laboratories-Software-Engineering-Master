using BillPath.Modern.ResourceBinders;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern.Controls.Edit
{
    public sealed partial class AmountView
        : UserControl
    {
        public AmountView()
        {
            InitializeComponent();
        }

        private void CurrencyComboBoxLoaded(object sender, RoutedEventArgs e)
            => CurrencyComboBox.SelectedItem = Application.Current.GetResource<SettingsViewModel>().PreferredCurrency;
    }
}