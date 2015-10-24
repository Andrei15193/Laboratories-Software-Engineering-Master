using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public sealed partial class DashboardPage
        : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private async void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var command = (AsyncCommand)((MenuFlyoutItem)sender).CommandParameter;

            await command.ExecuteAsync(new IncomeViewModel(
                new Models.Income
                {
                    Amount = new Models.Amount(1, new Models.Currency(new System.Globalization.RegionInfo("en-AU")))
                }));
        }
    }
}