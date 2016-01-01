using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BillPath.UserInterface.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BillPath.Modern
{
    public sealed partial class ExpensesPageView
        : UserControl
    {
        private enum ViewState
        {
            Invalid,
            Loading,
            Presenting
        }

        private ViewState _currentViewState;
        private Lazy<ExpensesPageViewModel> _viewModel;

        public ExpensesPageView()
        {
            _currentViewState = ViewState.Invalid;
            _viewModel = new Lazy<ExpensesPageViewModel>(() => (ExpensesPageViewModel)DataContext);
            InitializeComponent();
            Loaded += delegate
            {
                _viewModel.Value.PropertyChanged += delegate
                {
                    _EnsureVisualState();
                };
                _EnsureVisualState();
            };
        }

        private ExpensesPageViewModel _ViewModel
        {
            get
            {
                return _viewModel.Value;
            }
        }

        private void _EnsureVisualState()
        {
            if (_ViewModel.Loading)
                _TransitionTo(ViewState.Loading);
            else
                _TransitionTo(ViewState.Presenting);
        }

        private void _TransitionTo(ViewState viewState)
        {
            if (viewState == ViewState.Invalid)
                throw new ArgumentException(
                    "Cannot navigate to " + nameof(ViewState.Invalid) + " " + nameof(ViewState),
                    nameof(viewState));

            if (_currentViewState != viewState && VisualStateManager.GoToState(this, viewState.ToString(), true))
                _currentViewState = viewState;
        }

        private void _PageNumberKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter
                && GoToPageFlyoutButton.IsEnabled
                && GoToPageFlyoutButton.Command.CanExecute(null))
                GoToPageFlyoutButton.Command.Execute(null);
        }

        private void _ShowExpenseEditFlyout(object sender, ItemClickEventArgs e)
        {
            ExpenseEditGrid.DataContext = e.ClickedItem;
            var flyout = (Flyout)FlyoutBase.GetAttachedFlyout(ExpensesListView);
            flyout.ShowAt(ExpensesListView);
        }

        private async void _ExpenseFlyoutClosed(object sender, object e)
        {
            var expenseViewModel = (ExpenseViewModel)ExpenseEditGrid.DataContext;
            if (expenseViewModel.UpdateCommand.CanExecute)
                await expenseViewModel.UpdateCommand.ExecuteAsync(null);
            else if (expenseViewModel.RevertChangesCommand.CanExecute)
                expenseViewModel.RevertChangesCommand.Execute(null);

            ExpenseEditGrid.DataContext = null;
        }
    }
}