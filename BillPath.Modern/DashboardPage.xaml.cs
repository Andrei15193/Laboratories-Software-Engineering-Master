using System;
using System.Linq;
using BillPath.Models;
using BillPath.Modern.Mocks;
using BillPath.Modern.ResourceBinders;
using BillPath.Providers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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

        private void _ClickedExpenseCategory(object sender, ItemClickEventArgs e)
        {
            ExpenseCategoryEditControl.DataContext = e.ClickedItem;
            var frameworkElement = (FrameworkElement)sender;
            var flyout = (Flyout)FlyoutBase.GetAttachedFlyout(frameworkElement);
            flyout.ShowAt(frameworkElement);
        }

        private async void _ExpenseCategoryEditFlyoutClosed(object sender, object e)
        {
            var expenseCategoryViewModel = ExpenseCategoryEditControl.DataContext as UserInterface.ViewModels.ExpenseCategoryViewModel;
            if (expenseCategoryViewModel?.UpdateCommand.CanExecute ?? false)
                await expenseCategoryViewModel.UpdateCommand.ExecuteAsync(null);

            ExpenseCategoryEditControl.DataContext = null;
        }

        private void _HideAddExpenseFlyout(object sender, RoutedEventArgs e)
            => AddExpenseButtonFlyout.Hide();
        private void _AddExpenseFlyoutOpened(object sender, object e)
            => AddExpenseStackPanel.DataContext =
            new ExpenseViewModel
            {
                ModelState = ModelState.GetFor(
                    new Expense
                    {
                        Amount = new Amount(
                          0,
                          Application.Current.GetResource<SettingsViewModel>().PreferredCurrency),
                        DateRealized = DateTimeOffset.Now.Date
                    })
            };
    }
}