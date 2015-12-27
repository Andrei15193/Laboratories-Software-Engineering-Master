using System;
using BillPath.UserInterface.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace BillPath.Modern
{
    public sealed partial class IncomesPageView
        : UserControl
    {
        private enum ViewState
        {
            Invalid,
            Loading,
            Presenting
        }

        private ViewState _currentViewState;
        private Lazy<IncomesPageViewModel> _viewModel;

        public IncomesPageView()
        {
            _currentViewState = ViewState.Invalid;
            _viewModel = new Lazy<IncomesPageViewModel>(() => (IncomesPageViewModel)DataContext);
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


        private IncomesPageViewModel _ViewModel
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

        private void _ShowIncomeEditFlyout(object sender, ItemClickEventArgs e)
        {
            IncomeEditGrid.DataContext = e.ClickedItem;
            var flyout = (Flyout)FlyoutBase.GetAttachedFlyout(IncomesListView);
            flyout.ShowAt(IncomesListView);
        }

        private async void _IncomeFlyoutClosed(object sender, object e)
        {
            var incomeViewModel = (IncomeViewModel)IncomeEditGrid.DataContext;
            if (incomeViewModel.UpdateCommand.CanExecute)
                await incomeViewModel.UpdateCommand.ExecuteAsync(null);
            else if (incomeViewModel.RevertChangesCommand.CanExecute)
                incomeViewModel.RevertChangesCommand.Execute(null);

            IncomeEditGrid.DataContext = null;
        }
    }
}