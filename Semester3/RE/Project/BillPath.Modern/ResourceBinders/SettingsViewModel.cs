using BillPath.DataAccess;
using Windows.UI.Xaml;

namespace BillPath.Modern.ResourceBinders
{
    internal class SettingsViewModel
        : UserInterface.ViewModels.SettingsViewModel
    {
        public SettingsViewModel()
            : base(Application.Current.GetResource<ISettingsRepository>(nameof(SettingsRepository)))
        {
        }
    }
}