using Windows.UI.Xaml;
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

        private void _HideAddIncomeFlyout(object sender, RoutedEventArgs e)
            => AddIncomeButtonFlyout.Hide();
    }
}