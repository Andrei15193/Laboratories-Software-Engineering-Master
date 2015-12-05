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
        private Lazy<IncomesPageViewModel> _viewModel;

        public IncomesPagedView()
        {
            _currentViewState = ViewState.Invalid;
            _viewModel = new Lazy<IncomesPageViewModel>(() => (IncomesPageViewModel)DataContext);
            InitializeComponent();
            SelectedIncomes = IncomesListView.SelectedItems.Cast<Income>();
            Loaded += delegate
            {
                _viewModel.Value.PropertyChanged += delegate
                {
                    _EnsureVisualState();
                };
                _EnsureVisualState();
            };
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

        private IncomesPageViewModel _ViewModel
        {
            get
            {
                return _viewModel.Value;
            }
        }

        private void _ExecutingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (nameof(AsyncCommand.Executing).Equals(e.PropertyName, StringComparison.OrdinalIgnoreCase))
            //    _EnsureVisualState();
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
    }
}