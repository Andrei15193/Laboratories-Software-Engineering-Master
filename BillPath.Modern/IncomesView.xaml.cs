using System;
using System.ComponentModel;
using BillPath.UserInterface.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BillPath.Modern
{
    public sealed partial class IncomesView
        : UserControl
    {
        private enum ViewState
        {
            Loading = 0,
            Presenting
        }

        private ViewState _currentViewState;

        public IncomesView()
        {
            _currentViewState = (ViewState)(-1);
            InitializeComponent();
        }

        private async void _LoadedIncomesListView(object sender, RoutedEventArgs e)
        {
            var incomesViewModel = (IncomesViewModel)DataContext;

            TransitionTo(ViewState.Loading);

            await incomesViewModel.LoadPageInfoCommand.ExecuteAsync(null);
            await incomesViewModel.SelectPageCommand.ExecuteAsync(1);

            incomesViewModel.LoadPageInfoCommand.PropertyChanged += _ExecutingPropertyChanged;
            incomesViewModel.SelectPageCommand.PropertyChanged += _ExecutingPropertyChanged;

            _EnsureVisualState();
        }

        private void _ExecutingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(AsyncCommand.Executing).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                _EnsureVisualState();
        }

        private void _EnsureVisualState()
        {
            var incomesViewModel = (IncomesViewModel)DataContext;
            if (incomesViewModel.SelectPageCommand.Executing || incomesViewModel.LoadPageInfoCommand.Executing)
                TransitionTo(ViewState.Loading);
            else
                TransitionTo(ViewState.Presenting);
        }

        private void TransitionTo(ViewState viewState)
        {
            if (_currentViewState != viewState && VisualStateManager.GoToState(this, viewState.ToString(), true))
                _currentViewState = viewState;
        }

        private async void _SelectedPage(object sender, ItemClickEventArgs e)
        {
            await ((IncomesViewModel)DataContext).SelectPageCommand.ExecuteAsync(e.ClickedItem);
        }
    }
}