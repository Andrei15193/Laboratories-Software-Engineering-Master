using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.Mocks
{
    internal class SettingsViewModel
        : UserInterface.ViewModels.SettingsViewModel
    {
        public SettingsViewModel()
            : base((ISettingsRepository)Application.Current.Resources[nameof(SettingsRepository)])
        {
        }
    }
}