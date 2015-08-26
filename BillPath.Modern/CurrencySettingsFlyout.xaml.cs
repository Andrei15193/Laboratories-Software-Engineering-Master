using System.Diagnostics;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public sealed partial class CurrencySettingsFlyout
        : SettingsFlyout
    {
        public CurrencySettingsFlyout()
        {
            InitializeComponent();
        }

        private async void _SaveSettings(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Called " + nameof(_SaveSettings));

            var settingsViewModel = DataContext as SettingsViewModel;
            if (settingsViewModel != null && settingsViewModel.SaveCommand.CanExecute)
                await settingsViewModel.SaveCommand.ExecuteAsync(null);
        }
    }
}