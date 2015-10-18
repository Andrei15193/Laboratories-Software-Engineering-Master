using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BillPath.Models;
using BillPath.UserInterface.ViewModels;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BillPath.Modern
{
    public sealed partial class IncomesPagedView
        : UserControl
    {
        private enum ViewState
        {
            Invalid,
            Loading,
            Presenting
        }

        private ViewState _currentViewState;
        private Lazy<PaginationViewModel<Income>> _viewModel;

        public IncomesPagedView()
        {
            _currentViewState = ViewState.Invalid;
            _viewModel = new Lazy<PaginationViewModel<Income>>(() => (PaginationViewModel<Income>)DataContext);
            InitializeComponent();
            SelectedIncomes = IncomesListView.SelectedItems.Cast<Income>();
        }

        public IEnumerable<Income> SelectedIncomes
        {
            get;
        }
        public event SelectionChangedEventHandler IncomeSelectionChanged
        {
            add
            {
                IncomesListView.SelectionChanged += value;
            }
            remove
            {
                IncomesListView.SelectionChanged -= value;
            }
        }

        private PaginationViewModel<Income> _ViewModel
        {
            get
            {
                return _viewModel.Value;
            }
        }

        private async void _LoadedIncomesListView(object sender, RoutedEventArgs e)
        {
            _TransitionTo(ViewState.Loading);

            await _ViewModel.LoadCommand.ExecuteAsync(null);
            await _ViewModel.GoToPageCommand.ExecuteAsync(1);

            _ViewModel.LoadCommand.PropertyChanged += _ExecutingPropertyChanged;
            _ViewModel.GoToPageCommand.PropertyChanged += _ExecutingPropertyChanged;
            _ViewModel.GoToNextPageCommand.PropertyChanged += _ExecutingPropertyChanged;
            _ViewModel.GoToPreviousPageCommand.PropertyChanged += _ExecutingPropertyChanged;

            _TransitionTo(ViewState.Presenting);
        }

        private void _ExecutingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(AsyncCommand.Executing).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
                _EnsureVisualState();
        }

        private void _EnsureVisualState()
        {
            if (_ViewModel.GoToPageCommand.Executing ||
                _ViewModel.GoToNextPageCommand.Executing ||
                _ViewModel.GoToPreviousPageCommand.Executing ||
                _ViewModel.LoadCommand.Executing)
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

        private void _PageNumberTextChanged(object sender, TextChangedEventArgs e)
        {
            int pageNumber;

            if (int.TryParse(PageNumberTextBox.Text, out pageNumber)
                && 1 <= pageNumber
                && pageNumber <= _ViewModel.PageCount)
            {
                GoToPageFlyoutButton.IsEnabled = true;
                GoToPageFlyoutButton.CommandParameter = pageNumber;
            }
            else
                GoToPageFlyoutButton.IsEnabled = false;
        }

        private void _PageNumberKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter
                && GoToPageFlyoutButton.IsEnabled
                && GoToPageFlyoutButton.Command.CanExecute(GoToPageFlyoutButton.CommandParameter))
                GoToPageFlyoutButton.Command.Execute(GoToPageFlyoutButton.CommandParameter);
        }
    }
}