using System.Linq;
using BillPath.Models;
using BillPath.Modern.Mocks;
using BillPath.Modern.ResourceBinders;
using BillPath.Providers;
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

        private void _AddIncomeFlyoutOpened(object sender, object e)
            => AddIncomeStackPanel.DataContext =
            new IncomeViewModel
            {
                ModelState = new IncomeModelState()
            };

        private void _HideAddExpenseCategoryFlyout(object sender, RoutedEventArgs e)
            => AddExpenseCategoryButtonFlyout.Hide();
        private void _AddExpenseCategoryFlyoutOpened(object sender, object e)
            => AddExpenseCategoryStackPanel.DataContext =
            new ExpenseCategoryViewModel
            {
                ModelState = ModelState.GetFor(
                    new ExpenseCategory
                    {
                        Color = Application.Current.GetResource<ArgbColorProvider>().ArgbColors.First()
                    })
            };
    }
}